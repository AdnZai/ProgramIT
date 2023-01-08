const http = {
    log: (action, detail, data) => {
        http.post(`/Shared/LogAction`, { action: action, detail: detail, jsonData: data });
    },
    post: async (url, postData = {}) => {
        try {
            postData.scannerInfo = {
                number: app.scanner.number,
                user: {
                    code: app.user?.code ?? 0,
                    passwordHash: app.user?.passwordHash ?? null
                }
            };

            url = new URL(`${location.origin}${url}`);

            const response = await fetch(url, {
                method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData)
            });

            if (!response.ok) {
                return new Error(response.statusText);
            }

            const result = await response.json();

            if (result !== null && result.error !== undefined) {
                return new Error(result.error);
            }
            return result;
        }
        catch (ex) {
            if (ex.message === "Failed to fetch") {
                return new Error(`Brak połączenia z serwerem`);
            }
            return new Error(`Exception (http.post):<pre class="mb-0">${ex.stack}</pre>`);
        }
    },
    get: async (url = '', params = {}) => {
        url = new URL(`${location.origin}${url}`);
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]));
        try {
            const response = await fetch(url);
            const result = response.json();

            if (result.error !== undefined) {
                return new Error(result.error);
            }

            return result;
        }
        catch (ex) {
            if (ex.message === "Failed to fetch") {
                return new Error(`Brak połączenia z serwerem.`);
            }
            return new Error(`Exception (http.get):<pre class="mb-0">${ex.stack}</pre>`);
        }
    },
    getText: async (url = '', params = {}) => {
        url = new URL(`${location.origin}${url}`);
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]));
        try {
            const response = await fetch(url);
            return response.text();
        }
        catch (ex) {
            if (ex.message === "Failed to fetch") {
                return new Error(`Brak połączenia z serwerem.`);
            }
            return new Error(`Exception (http.getText):<pre class="mb-0">${ex.stack}</pre>`);
        }
    }
};
