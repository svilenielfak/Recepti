export class SastojakMera {

    constructor() {
        this.id = null;
        this.naziv = null;
        this.mera = null;
        this.container = null;
        this.deleted = false;
    }




    crtaj(host) {
        let newDiv = document.createElement("div");
        host.appendChild(newDiv);

        this.container = newDiv;

        newDiv.style.order = "-1";

        let txtbx = document.createElement("input");
        txtbx.className = "SastojakTxt";
        if (this.naziv === null)
            txtbx.placeholder = "Sastojak"
        else
            txtbx.value = this.naziv;
        newDiv.appendChild(txtbx);

        txtbx = document.createElement("input");
        txtbx.className = "MeraTxt";
        if (this.txtbx === null)
            txtbx.placeholder = "Mera"
        else
            txtbx.value = this.mera;
        newDiv.appendChild(txtbx);

        let lbl = document.createElement("label");
        lbl.innerHTML = "X";
        lbl.onclick = (ev) => {
            host.removeChild(newDiv);
            this.deleted = true;
        }
        newDiv.appendChild(lbl);
    }


    loadData() {
        if (this.deleted)
            return;

        this.naziv = this.container.querySelector(".SastojakTxt").value.trim();
        this.mera = this.container.querySelector(".MeraTxt").value.trim();
        if (this.naziv.length === 0 || this.mera.length === 0) {
            this.deleted = true;
            this.container.parentNode.removeChild(this.container);
        }
    }


}