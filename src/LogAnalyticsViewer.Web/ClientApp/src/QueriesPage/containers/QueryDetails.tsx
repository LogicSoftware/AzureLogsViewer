import React from "react";
import { QueryEntity } from '../types';
import { Classes, Checkbox, Button } from "@blueprintjs/core";
import { useQueryDetailsState } from "../state/queriesReducer"
import styles from "./QueryDetails.module.css";
import TextareaAutosize from 'react-textarea-autosize';

type Props = {
    query: QueryEntity;
    save: (query: QueryEntity) => void
    cancel: () => void;
}

export const QueryDetails: React.FC<Props> = ({ query, save, cancel }) => {
    const { state, actions } = useQueryDetailsState(query);

    return (
        <>
            <div className={styles.field}>
                <div className={styles.field_label}>{"Display name:"}</div>
                <input
                    value={state.data?.displayName}
                    className={Classes.INPUT}
                    onChange={(event) => actions.updateDisplayName(event.target.value)}
                />
                <div className={styles.field_label}>{"Chanel:"}</div>
                <input
                    value={state.data?.channel}
                    className={Classes.INPUT}
                    onChange={(event) => actions.updateChannelFilter(event.target.value)}
                />
                <div className={styles.field_label}>{"Query Text:"}</div>
                <TextareaAutosize
                    value={state.data?.queryText}
                    className={Classes.TEXT_LARGE}
                    onChange={(event) => actions.updateQueryTextFilter(event.target.value)}
                />
                <div className={styles.field_label}>{"Enabled:"}</div>
                <Checkbox
                    className={Classes.CHECKBOX}
                    checked={state.data?.enabled}
                    onClick={() => actions.updateEnabledFilter(!state.data.enabled)}
                />
            </div>
            <div className={styles.row}>
                <Button intent={"primary"} text={"Save"} style={{ width: 100 }} onClick={() => save(state.data)} />
                <Button intent={"none"} text={"Cancel"} style={{ width: 100 }} className={styles.cancel} onClick={cancel} />
            </div>
        </>
    )
}