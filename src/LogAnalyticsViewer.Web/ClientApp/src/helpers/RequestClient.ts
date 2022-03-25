import axios from "axios";

export class RequestClient {
    
    public static postJson(url: string, data?: any) {
        const source = axios.CancelToken.source();

        return axios.post(url, data, {
            cancelToken: source.token,
            withCredentials: true,
        });
    }
}

