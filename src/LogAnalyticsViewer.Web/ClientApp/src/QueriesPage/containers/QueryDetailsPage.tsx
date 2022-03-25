import React from "react";
import { QueryEntity } from '../types';
import { useFetchData } from "../../hooks/useFetchData";
import { appUrls } from "../../appUrls";
import { RouteComponentProps } from "react-router-dom";
import { QueryDetails } from "./QueryDetails"
import { RequestClient } from "../../helpers/RequestClient"

type RouteParams = {
    id?: string
};

type Props = RouteComponentProps<RouteParams>

export const QueryDetailsPage: React.FC<Props> = (props) => {
    const id = props.match.params && props.match.params.id ? parseInt(props.match.params.id, 10) : null;

    const { result } = useFetchData({
        url: appUrls.get,
        payload: {
            "QueryId": id,
        }
    }, []);

    const query: QueryEntity = result;

    if (!query) {
        return null;
    }

    const save = (query: QueryEntity) => {
        var promise = RequestClient.postJson(appUrls.update, query);
        promise.then(() => {
            props.history.push('/queries');
        });
    }

    const cancel = () => {
        props.history.push('/queries');
    }

    return (
        <>
            <QueryDetails cancel={cancel} save={save} query={query} />
        </>
    )
}