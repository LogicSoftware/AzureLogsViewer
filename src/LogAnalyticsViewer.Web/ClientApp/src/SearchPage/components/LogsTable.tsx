import { Classes, IconName, NonIdealState, Spinner } from "@blueprintjs/core";
import React from "react";
import classNames from "classnames";
import moment from "moment";
import styles from "./LogsTable.module.css";

type Event = {
    timeGenerated: Date;
    message: string;
    source: string;
};


export const stubEvents = Array.from(Array(100).keys()).map(id => ({
    timeGenerated: new Date(),
    message: `EventName="ObjectEvent" Object="Timestamp: 1/23/2020 8:49:42 AM
Message: Invalid ticket hash: '423d03fca507d06d6ed0e9b4d24f0008'
Category: 
Priority: -1
EventId: 0
Severity: Warning
Title:ExtractAuthDataFromTicket
Machine: RD0003FFBC538B
App Domain: /LM/W3SVC/1273337584/ROOT-1-132241717301707072
ProcessId: 4588
Process Name: d:\\windows\\system32\\inetsrv\\w3wp.exe
Thread Name: 
Win32 ThreadId:2684
Extended Properties: User Details - IsAnonymous 
 AccountId=27314, Account Name = TopBagage
" TraceSource="All Events"`,
    source: "core",
} as Event));


type Props = {
    events: Event[];
    loading?: boolean;
    isError?: boolean;
};

export const LogsTable: React.FC<Props> = ({ loading, events, isError }) => {
    return (
        <>
            {!!loading && (
                <div className={styles.spinner_wrapper}>
                    <Spinner size={200} />
                </div>
            )}
            <LogsTableInner events={events} isError={isError}  />
        </>
    )
};

const LogsTableInner: React.FC<Props> = ({ events, isError }) => {
    if(!events?.length || isError) {
        const isDataLoaded = !!events;
        let title = isDataLoaded ? "No search results" : "Setup filters and press apply";
        let icon: IconName = "search";
        if(isError) {
            title = "Error";
            icon = "error";
        }
        return (
            <div className={styles.spinner_wrapper}>
                <NonIdealState title={title} icon={icon} />
            </div>
        )
    }
    
    const tableClassNames = classNames(
        Classes.HTML_TABLE,
        Classes.HTML_TABLE_BORDERED,
        Classes.HTML_TABLE_STRIPED,
        styles.events_table
    );
    return (
        <>
            <table className={tableClassNames}>
                <thead>
                    <tr>
                        <th>Time Generated</th>
                        <th>Message</th>
                        <th>Source</th>
                    </tr>
                </thead>
                <tbody>
                {events.map((event, index) => (
                    <tr key={index}>
                        <td>{moment(event.timeGenerated).format("YYYY-MM-DD HH:mm:ss")}</td>
                        <td className={styles.event_message}>{event.message}</td>
                        <td>{event.source}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </>
    )
};