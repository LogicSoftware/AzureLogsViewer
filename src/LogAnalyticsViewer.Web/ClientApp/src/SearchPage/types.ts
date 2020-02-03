export type MessageFilterType = "like" | "notlike";

export type MessageFilter = {
    id?: number;
    text: string;
    type: MessageFilterType;
};

export type SearchFilters = {
    from?: Date;
    to?: Date;
    queryId?: number;
    messageFilters: MessageFilter[];
}

export type UrlSearchFiltersParams = {
    from: string;
    to: string;
    queryId: number;
}   