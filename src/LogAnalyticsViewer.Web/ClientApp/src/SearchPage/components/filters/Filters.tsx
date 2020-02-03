import { Button, NumericInput } from "@blueprintjs/core";
import React from "react";
import { useFiltersState } from "../../state/filtersReducer";
import { SearchFilters, UrlSearchFiltersParams } from "../../types";
import { AddMessageFilter } from "./AddMessageFilter";
import { DateFilterInput } from "./DateFilterInput";
import styles from "./Filters.module.css";
import { MessageEditor } from "./MessageEditor";
import moment from "moment";
import { getURLParametersFromSearch } from "../../../helpers/getURLParametersFromSearch";
import { useLocation } from "react-router-dom";

type Props = {
    onChange: (filter: SearchFilters) => any;
};

export const Filters: React.FC<Props> = ({ onChange }) => {
    const { state, actions } = useFiltersState();
    const messages = React.useMemo(() => Object.values(state.messageFilters), [state.messageFilters]);
    const onApply = () => {
        const messageFilters = Object.values(state.messageFilters)
                                     .map(x => ({...x, text: x.text.trim()}))
                                     .filter(x =>  x.text);
        

        const {ignoreForceApply, ignoreLocation, ...other} = state;

        const filters: SearchFilters = {
            ...other,
            from: state.from && moment(state.from).utc(true).toDate(),
            to: state.to &&  moment(state.to).utc(true).toDate(),
            messageFilters,
        };
        
        onChange(filters);
    };
    
    const onChangeQueryId = (value: number) => {
        if (!value){
            actions.setQueryId(undefined);
            return;
        }

        actions.setQueryId(value);
    }

    const location = useLocation();
    const params = getURLParametersFromSearch(location.search) as UrlSearchFiltersParams;

    if (params && !state.ignoreLocation) {
        actions.setFrom(params.from && moment(params.from).toDate());
        actions.setTo(params.to && moment(params.to).toDate());
        actions.setQueryId(params.queryId && Number(params.queryId));
        actions.setIgnoreLocation(true);
    }

    if (state.ignoreLocation && !state.ignoreForceApply) {
        onApply();
        actions.setIgnoreForceApply(true);
    }

    return (
        <div>
            <div className={styles.row}>
                <span>From </span>
                <DateFilterInput value={state.from} onChange={actions.setFrom} />
                <span className={styles.indent} />
                <span>To </span>
                <DateFilterInput value={state.to} onChange={actions.setTo} />
                <span className={styles.indent} />
                <span>QueryId </span>
                <div className={styles.numeric_field}>
                    <NumericInput className={"pt-fill"} value={state.queryId} onValueChange={onChangeQueryId} />
                </div>
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


