
export interface KeyOfString<T> {
    [key: string]: T;
}

export const getURLParametersFromSearch = (queryString: string) => {
    const params: KeyOfString<any> = {};
    const pairs = (queryString[0] === "?" ? queryString.substr(1) : queryString).split("&");

    if (pairs[0] === ""){
        return null;
    }

    for (const element of pairs) {
        const pair = element.split("=");
        params[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1] || "");
    }
    return params;
};
