import React from "react";
import { QueryEntity } from '../types';
import { appUrls } from "../../appUrls";
import { RouteComponentProps } from "react-router-dom";
import { QueryDetails } from "./QueryDetails"
import { RequestClient } from "../../helpers/RequestClient"

type RouteParams = {
    id?: string
};

type Props = RouteComponentProps<RouteParams>

export const QueryCreatePage: React.FC<Props> = (props) => {

    const create = (query: QueryEntity) => {
        var promise = RequestClient.postJson(appUrls.create, query);
        promise.then(() => {
            props.history.push('/queries');
        });
    }

    const cancel = () => {
        props.history.push('/queries');
    }

    return (
        <>
            <QueryDetails cancel={cancel} save={create} query={null} />
        </>
    )
}