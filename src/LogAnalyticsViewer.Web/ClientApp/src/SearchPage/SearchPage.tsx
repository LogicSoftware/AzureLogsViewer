import React from "react";
import { Filters } from "./components/Filters";
import { LogsTable } from "./components/LogsTable";

export const SearchPage: React.FC = () => {
    return (
        <div>
            <Filters />
            <LogsTable />
        </div>
    );
};