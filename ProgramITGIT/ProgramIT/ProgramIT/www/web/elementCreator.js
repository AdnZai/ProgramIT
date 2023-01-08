class E
{


    static create(tag)
    {
        const creator = new E();
        creator._tag = tag;
        return creator;
    }

    static load(name, p = {})
    {
        const template = document.createElement(`template`);
        template.innerHTML = htmls.get(name, p);
        const creator = new E();
        creator.createdElement = template.content.children[0];
        return creator;
    }

    appendTo(element)
    {
        this._appendTo = element;
        return this;
    }

    prependTo(element)
    {
        this._prependTo = element;
        return this;
    }

    className(className)
    {
        this._className = className;
        return this;
    }

    innerHTML(innerHTML)
    {
        this._innerHTML = innerHTML;
        return this;
    }

    onclick(onclick)
    {
        this._onclick = onclick;
        return this;
    }

    onsubmit(onsubmit)
    {
        this._onsubmit = onsubmit;
        return this;
    }

    loadElementsTo(target)
    {
        this._controlsTarget = target;
        return this;
    }

    build()
    {
        if (this._tag != null)
        {
            this.createdElement = document.createElement(this._tag);
        }

        if (this._innerHTML != null)
        {
            this.createdElement.innerHTML = this._innerHTML;
        }

        if (this._tag === "template")
            this.createdElement = this.createdElement.content.children[0];

        if (this._appendTo != null)
        {
            this._appendTo.append(this.createdElement);
        }
        else if (this._prependTo != null)
        {
            this._prependTo.prepend(this.createdElement);
        }

        if (this._className != null)
        {
            this.createdElement.className = this._className;
        }

        if (this._onclick != null)
        {
            this.createdElement.onclick = this._onclick;
        }

        if (this._onsubmit != null)
        {
            this.createdElement.onsubmit = this._onsubmit;
        }

        if (this._controlsTarget != null)
        {
            const controls = this.createdElement.querySelectorAll("[data-element]");
            controls.forEach(control =>
            {
                this._controlsTarget[control.dataset.element] = control;
            });
        }

        return this.createdElement;
    }

}
