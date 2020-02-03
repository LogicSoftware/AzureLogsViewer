import { Button } from "@blueprintjs/core";
import React from "react";
import { useFiltersState } from "../../state/filtersReducer";
import { SearchFilters } from "../../types";
import { AddMessageFilter } from "./AddMessageFilter";
import { DateFilterInput } from "./DateFilterInput";
import styles from "./Filters.module.css";
import { MessageEditor } from "./MessageEditor";
import moment from "moment";

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
        
        const filters: SearchFilters = {
            ...state,
            from: state.from && moment(state.from).utc(true).toDate(),
            to: state.to &&  moment(state.to).utc(true).toDate(),
            messageFilters,
        };
        
        onChange(filters);
    };
    
    // TODO : init from query string
    return (
        <div>
            <div className={styles.row}>
                <span>From </span>
                <DateFilterInput value={state.from} onChange={actions.setFrom} />
                <span className={styles.indent} />
                <span>To </span>
                <DateFilterInput value={state.to} onChange={actions.setTo} />
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


