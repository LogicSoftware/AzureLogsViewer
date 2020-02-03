import { DependencyList, useEffect, useState } from "react";
import axios from "axios";

type Params = {
    url: string;
    payload?: any;
    condition?: boolean;
};

export const useFetchData = <T>(params: Params, deps: DependencyList) => {
    const [result, setResult] = useState<T>();
    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);
    
    useEffect(() => {
        if(params.condition === false) {
            return;
        }
        
        const source = axios.CancelToken.source();
        const loadData = async () => {
            try {
                setIsLoading(true);
                setIsError(false);
                const resp = await axios.post(params.url, params.payload, {
                    cancelToken: source.token,
                    withCredentials: true,
                    transformResponse: parseJson,
                });

                setResult(resp.data);
            }
            catch(e) {
                if(!axios.isCancel(source.token)) {
                    setIsError(true);
                }       
            } finally {
                setIsLoading(false);
            }
            
        };
        loadData();
        
        return () => {
            source.cancel();
        }
        
    }, deps);
    
    return { result, isLoading, isError };
};

// IE/FF/Chrome handle ISO date format in Date.parse inconsistently so we need to have our own implementation
const isoDateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:\.(\d*))?Z?$/;

const fromISOString = function (date: string) {
    let parts = isoDateRegex.exec(date);
    if (parts) {
        var ms = +((parts[7] || "") + "000").substring(0, 3); // handling 1/10 and 1/100 and trimming extra digits (may come from C# DateTime)

        return new Date(Date.UTC(+parts[1], +parts[2] - 1, +parts[3], +parts[4], +parts[5], +parts[6], ms)); // always UTC
    }

    return null;
};

const parseJson = (json: string) => {
    if (!json) {
        return null;
    }

    function reviver(key: string, value: any) {
        if (typeof value === 'string') {
            var date = fromISOString(value);
            if (date) {
                // treating parsed date as local client date
                return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds(), date.getUTCMilliseconds());
            }
        }
        return value;
    }

    return JSON.parse(json, reviver);
}