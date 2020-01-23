import { act } from "react-dom/test-utils";

let messageIdCounter = 1;

type MessageFilterType = "like" | "notlike";

export type FiltersState = {
    from?: Date;
    to?: Date;
    queryId?: number;
    messageFilters: {
        [key: string]: MessageFilter;
    }
}

export type MessageFilter = {
    id?: number;
    text: string;
    type: MessageFilterType;
};

export const filterActions = {
    update: (data: Partial<Omit<FiltersState, "messageFilters">>) => ({
        type: "Update", 
        data,
    } as const),
    
    setTo: (to: Date) => filterActions.update({ to }),
    
    setFrom: (from: Date) => filterActions.update({ from }),
    
    setQueryId: (queryId: number) => filterActions.update({ queryId }),
    
    addMessageFilter: (type: MessageFilterType) => ({
        type: "AddMessageFilter",
        filterType: type,
    } as const),
    
    deleteMessageFilter: (id: number) => ({
        type: "DeleteMessageFilter",
        id
    } as const),
    
    updateMessageFilter: (id: number, text: string) => ({
        type: "UpdateMessageFilter",
        id,
        text,
    } as const)
};

type Actions = ReturnType<typeof filterActions.update> | 
               ReturnType<typeof filterActions.addMessageFilter> |
               ReturnType<typeof filterActions.deleteMessageFilter> |
               ReturnType<typeof filterActions.updateMessageFilter>;

export const filtersReducer = (state: FiltersState, action: Actions) => {
    switch (action.type) {
        case "Update": 
            return {...state, ...action.data };
        case "AddMessageFilter":
            const id = messageIdCounter++;
            return  {
                ...state,
                messageFilters: {
                    ...state.messageFilters,
                    [id]: { id, text: "", type: action.filterType },
                }
            };
        case "DeleteMessageFilter": {
            const messageFilters = { ...state.messageFilters };
            delete messageFilters[action.id];
            return {
                ...state,
                messageFilters: messageFilters,
            };
        }
        case "UpdateMessageFilter": {
            const messageFilters = { ...state.messageFilters };
            messageFilters[action.id] = {
                ...messageFilters[action.id],
                text: action.text,
            };
            return {
                ...state,
                messageFilters: messageFilters,
            };
        }
        default:
            return state;
    }
};
