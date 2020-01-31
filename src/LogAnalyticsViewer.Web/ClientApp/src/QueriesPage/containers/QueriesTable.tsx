import React from "react";
import { Classes, Spinner, Button } from "@blueprintjs/core";
import styles from "./QueriesTable.module.css";
import classNames from "classnames";
import { Link } from 'react-router-dom'
import { QueryEntity } from '../types';

type Props = {
    queries: QueryEntity[];
    isLoading?: boolean;
    create?: () => void;
};

export const QueriesTable: React.FC<Props> = ({ isLoading, ...props }) => {
    return (
        <>
            <QueriesTableInner {...props} />
            {!!isLoading && (
                <div className={styles.spinner_wrapper}>
                    <Spinner size={100} />
                </div>
            )}
        </>
    )
};

const QueriesTableInner: React.FC<Props> = ({ queries, create }) => {

    const tableClassNames = classNames(
        Classes.HTML_TABLE,
        Classes.HTML_TABLE_BORDERED,
        Classes.HTML_TABLE_STRIPED,
        styles.queries_table
    );

    return (
        <>
            <Button intent={"primary"} text={"Create"} style={{ width: 100 }} onClick={create} />
            <table className={tableClassNames}>
                <thead>
                    <tr>
                        <th>Display name</th>
                        <th>Chanel</th>
                        <th>Enabled</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {queries && queries.map((query, index) => (
                        <tr key={index}>
                            <td>{query.displayName}</td>
                            <td>{query.channel}</td>
                            <td>{query.enabled ? 'Enabled' : 'Dissabled'}</td>
                            <td><Link to={{ pathname: "/queries/" + query.queryId }}>Edit</Link></td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>
    )
};