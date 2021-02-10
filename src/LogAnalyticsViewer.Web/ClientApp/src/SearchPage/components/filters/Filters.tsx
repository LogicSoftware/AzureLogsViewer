import { Button, NumericInput } from "@blueprintjs/core";
import React, { useEffect, useState } from "react";
import { useFiltersState } from "../../state/filtersReducer";
import { SearchFilters, UrlSearchFiltersParams } from "../../types";
import { AddMessageFilter } from "./AddMessageFilter";
import { DateFilterInput } from "./DateFilterInput";
import styles from "./Filters.module.css";
import { MessageEditor } from "./MessageEditor";
import moment from "moment";
import { getURLParametersFromSearch } from "../../../helpers/getURLParametersFromSearch";
import { useLocation } from "react-router-dom";
import { QueryIdFilter } from "./QueryIdFilter";

type Props = {
    onChange: (filter: SearchFilters) => any;
};

export const Filters: React.FC<Props> = ({ onChange }) => {
    const [applyFromQuery, setApplyFromQuery] = useState(false);
    const { state, actions } = useFiltersState();
    const messages = React.useMemo(() => Object.values(state.messageFilters), [state.messageFilters]);
    const onApply = () => {
        const messageFilters = Object.values(state.messageFilters)
                                     .map(x => ({...x, text: x.text.trim()}))
                                     .filter(x =>  x.text);
        
        const filters: SearchFilters = {
            ...state,
            from: state.from && moment(state.from).utc(true).toDate(),
            to: state.to &&  moment(state.to).utc(true).toDate(),
            messageFilters,
        };
        
        onChange(filters);
    };

    const location = useLocation();
    useEffect(() => {
        let params = getURLParametersFromSearch(location.search) || {} as UrlSearchFiltersParams;
        let { from, to, queryId } = params;

        if (from || to || queryId) {
            if (from) {
                actions.setFrom(params.from && moment(params.from).toDate());
            }
            if (to) {
                actions.setTo(params.to && moment(params.to).toDate());
            }
            if (queryId) {
                actions.setQueryId(params.queryId && Number(params.queryId));
            }
            // postpone onApply to get actual state in onApply callback
            setApplyFromQuery(true);
        }
    }, []);

    useEffect(() => {
        if(applyFromQuery) {
            onApply();
        }
    }, [applyFromQuery]);

    return (
        <div>
            <div className={styles.row}>
                <span>From </span>
                <DateFilterInput value={state.from} onChange={actions.setFrom} />
                <span className={styles.indent} />
                <span>To </span>
                <DateFilterInput value={state.to} onChange={actions.setTo} />
                <span className={styles.indent} />
                <span>Query </span>
                <QueryIdFilter value={state.queryId} onChange={actions.setQueryId} />
            </div>
            {messages.map(m => (
                <div className={styles.row} key={m.id}>
                    <MessageEditor message={m} actions={actions} />
                </div>
            ))}
            <div className={styles.row}>
                <AddMessageFilter add={actions.addMessageFilter} />
                <span className={styles.indent} />
                <Button intent={"primary"} text={"Apply"} style={{width: 160 }} onClick={onApply} />
            </div>
            <br />
        </div>
    );
};


