export class KorakDrawer {

    constructor() {
        this.id = null;
        this.brKorak = null;
        this.opis = null;
        this.deleted = false;
        this.container = null;

    }


    crtaj(host) {
        let newDiv = document.createElement("div");
        newDiv.className = "KorakDiv";
        host.appendChild(newDiv);

        this.container = newDiv;

        let txtbx = document.createElement("textarea");
        txtbx.className = "KorakTxt";
        if (this.opis === null)
            txtbx.placeholder = "Opis"
        else
            txtbx.value = this.opis;
        newDiv.appendChild(txtbx);

        let lbl = document.createElement("label");
        lbl.innerHTML = "X";
        lbl.onclick = (ev) => {
            host.removeChild(newDiv);
            this.deleted = true;

        }
        newDiv.appendChild(lbl);
    }

    loadData(index) {
        if (this.deleted )
            return index;
        this.opis = this.container.querySelector(".KorakTxt").value.trim();
        if (this.opis.length === 0) {
            this.container.parentNode.removeChild(this.container);
            this.deleted = true;
            return index;
        }
        this.brKorak = index;
        return index + 1;
    }
}