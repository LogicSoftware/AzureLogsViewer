import React from "react";
import { useFetchData } from "../../../hooks/useFetchData";
import { appUrls } from "../../../appUrls";
import { QueryEntity } from "../../../QueriesPage/types";
import { HTMLSelect } from "@blueprintjs/core";

type Props = {
    value: number;
    onChange: (val: number) => any;
}

export const QueryIdFilter: React.FC<Props> = ({ value, onChange }) => {
    const { result } = useFetchData<QueryEntity[]>({
        url: appUrls.list,
    }, []);

    const onSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const val = parseInt(e.target.value);
        onChange(isNaN(val) ? null : val);
    }

    return (
        <HTMLSelect value={value} onChange={onSelectChange}>
            <option>None</option> 
            {result && result.map(i => (
                <option key={i.queryId} value={i.queryId}>{i.displayName}</option>
            ))}
        </HTMLSelect>
    );
};