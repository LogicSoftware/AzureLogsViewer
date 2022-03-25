import { QueryEntity } from "../types";
import { useMemo, useReducer } from "react";

export type QueryDetailsState = {
    data: QueryEntity,
}

export const queryDetailsActions = {
    updateChannelFilter: (text: string) => ({
        type: "updateChannel",
        text,
    } as const),
    updateDisplayName: (text: string) => ({
        type: "updateDisplayName",
        text,
    } as const),
    updateEnabledFilter: (enabled: boolean) => ({
        type: "updateEnabled",
        enabled,
    } as const),
    updateQueryTextFilter: (queryText: string) => ({
        type: "updateQueryText",
        queryText,
    } as const)
};

type Actions = ReturnType<typeof queryDetailsActions.updateChannelFilter> |
    ReturnType<typeof queryDetailsActions.updateDisplayName> |
    ReturnType<typeof queryDetailsActions.updateEnabledFilter> |
    ReturnType<typeof queryDetailsActions.updateQueryTextFilter>;

export const queryDetailsReducer = (state: QueryDetailsState, action: Actions) => {
    switch (action.type) {
        case "updateChannel": {
            return { ...state, data: { ...state.data, channel: action.text } };
        }
        case "updateDisplayName": {
            return { ...state, data: { ...state.data, displayName: action.text } };
        }
        case "updateEnabled": {
            return { ...state, data: { ...state.data, enabled: action.enabled } };
        }
        case "updateQueryText": {
            return { ...state, data: { ...state.data, queryText: action.queryText } };
        }
        default:
            return state;
    }
};

export const useQueryDetailsState = (query: QueryEntity) => {
    const [state, dispatch] = useReducer(queryDetailsReducer, { data: query });
    const actions = useMemo(() => {
        const result: any = {};
        Object.entries(queryDetailsActions).forEach(([key, action]: [string, any]) => {
            result[key] = (...args: any[]) => dispatch(action(...args));
        });

        return result as typeof queryDetailsActions;
    }, []);

    return { state, dispatch, actions };
}