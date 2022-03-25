import React from "react";
import { appUrls } from "../appUrls";
import { useFetchData } from "../hooks/useFetchData";
import { QueriesTable } from "./containers/QueriesTable";
import { RouteComponentProps } from "react-router-dom";

type RouteParams = {
    id?: string
};

type Props = RouteComponentProps<RouteParams>

export const QueriesPage: React.FC<Props> = (props) => {
    
    const { result: queries, isLoading } = useFetchData({
        url: appUrls.list,
    }, []);
    
    const create = () => {
        props.history.push('/queries/create');
    }

    return (
        <div>
            <QueriesTable create={create} isLoading={isLoading} queries={queries}/>
        </div>
    );
};