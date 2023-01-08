const htmls = {
    loadHtmls: async () => {
        const files = await http.get('/Data/Htmls');
        htmls.files = {};

        for (const file of files) {
            htmls.files[file.slice(0, -5)] = await http.getText(`/htmls/${file}`);
        }

        //loadingSteps++;
    }, get: (name, p = {}) => {
        const file = htmls.files[name];

        if (file === undefined)
            alert(`Html named ${name} doesn't exist`);

        return eval("`" + htmls.files[name] + "`");
    }, getElement: (name) => {
        return E.create(`template`).innerHTML(htmls.get(name)).build().content.firstChild;
    }, download: async (name) => {
        return await http.getText(`/htmls/${name}.html`);
    }
}
