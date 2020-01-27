import React, { useState } from "react";
import { appUrls } from "../appUrls";
import { useFetchData } from "../hooks/useFetchData";
import { Filters } from "./components/filters/Filters";
import { LogsTable } from "./components/LogsTable";
import { SearchFilters } from "./types";

export const SearchPage: React.FC = () => {
    const [filters, setFilters] = useState(null as SearchFilters);
    
    const { result: events, isLoading, isError } = useFetchData({
        condition: !!filters,
        url: appUrls.search,
        payload: {
            ...filters,
            messageFilters: filters?.messageFilters?.map(({text, type}) => ({ text, type }))
        },
    }, [filters]);
    
    return (
        <div>
            <Filters onChange={setFilters}/>
            <LogsTable loading={isLoading} isError={isError} events={events} />
        </div>
    );
};