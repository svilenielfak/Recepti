import { SastojakMera } from "./SastojakMera.js";
import { KorakDrawer } from "./KorakDrawer.js";
import { Korak } from "./Korak.js";

export class Recept {

    constructor(id, naziv, korisnik, ocena, ulogovaniKor, fetchBegin) {
        this.id = id;
        this.naziv = naziv;
        this.korisnik = korisnik;
        this.ocena = ocena;
        this.container = null;
        this.ulogovaniKor = ulogovaniKor;
        this.ocenio = 0;
        this.fetchBegin = fetchBegin;

    }


    crtajReceptOpis(host, isMyRecept) {
        this.container = host.parentNode.parentNode.parentNode;

        var tr = document.createElement("tr");
        host.appendChild(tr);

        var el = document.createElement("td");
        el.innerHTML = this.naziv;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML = this.ocena;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML = this.korisnik.ime + " " + this.korisnik.prezime;
        tr.appendChild(el);

        el = document.createElement("td");
        let btn = document.createElement("button");
        btn.innerHTML = "Pogledaj";
        btn.className = "PregledBtn";

        btn.onclick = (ev) => {
            if (this.ulogovaniKor !== null)
                fetch(this.fetchBegin + "ReceptOcena/DidOcenio/" + this.id + "/" + this.ulogovaniKor.id)
                    .then(response => {
                        if (response.ok) {
                            if (response !== null)
                                response.json().then(resp => {
                                    this.ocenio = resp.ocena;
                                    this.prikaziRecept(host.parentNode);
                                    return;
                                }).catch(er => {
                                    this.ocenio = 0;
                                    this.prikaziRecept(host.parentNode);
                                });
                        } else {
                            this.ocenio = 0;
                            this.prikaziRecept(host.parentNode);
                        }
                    });

            else {

                this.ocenio = 0;
                this.prikaziRecept(host.parentNode);

            }


        };
        el.appendChild(btn);
        tr.appendChild(el);

        if (isMyRecept) {
            el = document.createElement("td");
            let btn = document.createElement("button");
            btn.innerHTML = "Izmeni";
            btn.className = "IzmeniBtn";

            btn.onclick = (ev) => {
                fetch(this.fetchBegin + "Recept/Recept/" + this.id, { method: "GET" })
                    .then(response => {
                        if (response.ok) {
                            response.json().then(rsp => {
                                this.prikaziIzmenu(host.parentNode, rsp);

                            });
                        } else
                            throw response;
                    }).catch(error => error.text().then(errmsg => alert(errmsg)));
            };
            el.appendChild(btn);
            tr.appendChild(el);


            el = document.createElement("td");
            btn = document.createElement("button");
            btn.innerHTML = "Obrisi";
            btn.className = "ObrisiBtn";

            btn.onclick = (ev) => {
                fetch(this.fetchBegin + "Recept/Delete/" + this.id, { method: "DELETE" })
                    .then(response => {
                        if (response.ok) {
                            alert("Recept obrisan!");
                            host.removeChild(tr);
                        } else {
                        }
                    });
            };
            el.appendChild(btn);
            tr.appendChild(el);
        }



    }

    prikaziRecept(rm) {
        if (rm !== null && rm.parentNode !== null)
            rm.parentNode.removeChild(rm);


        var div;
        let lbl;

        if (this.ulogovaniKor !== null) {



            div = document.createElement("div");
            div.className = "ReceptShow";
            div.style.justifyContent = "center";


            let cmbbx = document.createElement("select");
            cmbbx.style.margin = "10px";
            cmbbx.name = "ocene";

            let ocena;
            for (let i = 1; i < 6; i++) {
                ocena = document.createElement("option");
                ocena.innerHTML = i;
                ocena.value = i;
                cmbbx.appendChild(ocena);
            }

            lbl = document.createElement("label");
            if (this.ocenio === 0)
                lbl.innerHTML = "Ocenite ovaj recept : ";
            else {
                lbl.innerHTML = "Promenite ocenu : ";
                cmbbx.value = this.ocenio;
            }
            lbl.style.margin = "10px";

            div.appendChild(lbl);




            div.appendChild(cmbbx);

            let btnOceni = document.createElement("button");
            btnOceni.innerHTML = "Oceni";
            btnOceni.onclick = (ev) => {
                this.oceni(cmbbx.value);
            };
            div.appendChild(btnOceni);

            this.container.appendChild(div);

        }



        div = document.createElement("div");
        div.className = "ReceptShow";

        lbl = document.createElement("h2");
        lbl.innerHTML = this.naziv;
        div.appendChild(lbl);
        this.container.appendChild(div);


        div = document.createElement("div");
        div.className = "ReceptShow";

        let ul = document.createElement("ul");
        var li;

        fetch(this.fetchBegin + "ReceptSastojak/Sastojci/" + this.id)
            .then(resp => {
                if (resp.ok) {
                    resp.json().then(response => {
                        response.forEach(element => {
                            li = document.createElement("li");
                            li.innerHTML = element.sastojak.naziv + " : " + element.mera;
                            li.value = element.id;
                            ul.appendChild(li);
                        });
                    });
                }
            });

        div.appendChild(ul);
        this.container.appendChild(div);


        div = document.createElement("div");
        div.className = "ReceptShow";
        div.style.flexDirection = "column";

        lbl = document.createElement("h3");
        lbl.innerHTML = "Priprema";
        lbl.style.alignSelf = "center";
        div.appendChild(lbl);

        this.container.appendChild(div);


        var ol = document.createElement("ol");
        fetch(this.fetchBegin + "ReceptKorak/PreuzmiSve/" + this.id)
            .then(resp => {
                if (resp.ok) {
                    resp.json().then(response => {
                        if (Array.isArray(response))
                            response.forEach(element => {
                                li = document.createElement("li");
                                li.innerHTML = element.opis;
                                ol.appendChild(li);
                            });
                        else {
                            li = document.createElement("li");
                            li.innerHTML = response.opis;
                            li.value = response.id;
                            ol.appendChild(li);
                        }
                    });
                }
            });

        div.appendChild(ol);

    }

    tryUpdateRecept(naziv, sastojci, koraci) {

        sastojci = sastojci
            .filter((sastojak) => { return sastojak.deleted == false });
        koraci = koraci
            .filter(korak => { return korak.deleted == false });

        let kor = [];
        koraci.forEach(korak =>{
            kor.push(new Korak(korak.brKorak, korak.opis))
        }) ;      

        var body = JSON.stringify({
            naziv: naziv,
            korisnikID: this.korisnik.id,
            sastojci: JSON.stringify(sastojci),
            koraci: JSON.stringify(kor)
        }).replace(/\\/g, '').replace(/\"\[/g, '[').replace(/\]\"/g, ']');


        fetch(this.fetchBegin + "Recept/Update", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: body
        }).then(resp => {
            if (resp.ok) {
                alert("Recept izmenjen!");
            } else {
                throw resp;
            }
        }).catch(err => {
            err.text().then(errMsg => alert(errMsg));
        })

    }

    prikaziIzmenu(rm, recept) {
        if (rm !== null && rm.parentNode !== null) {
            rm.parentNode.removeChild(rm);
        }

        let sastojci = [];
        let koraci = [];
        fetch(this.fetchBegin + "ReceptSastojak/Sastojci/" + recept.id, {
            method: "GET"
        }).then(response => {

            if (response.ok) {
                response.json().then(resp => {

                    resp.forEach(el => {
                        let s = new SastojakMera();
                        s.id = el.id;
                        s.naziv = el.sastojak.naziv;
                        s.mera = el.mera;
                        sastojci.push(s);
                    });

                    fetch(this.fetchBegin + "ReceptKorak/PreuzmiSve/" + recept.id)
                        .then(rspns => {

                            if (rspns.ok) {
                                rspns.json().then(rsp => {

                                    rsp.forEach(el => {
                                        let korak = new KorakDrawer();
                                        korak.id = el.id;
                                        korak.brKorak = el.brKorak;
                                        korak.opis = el.opis;
                                        koraci.push(korak);
                                    });

                                    let divDodavanje = document.createElement("div");
                                    divDodavanje.className = "DodavanjeDiv";
                                    this.container.appendChild(divDodavanje);

                                    let div = document.createElement("div");
                                    div.className = "DodavanjeNazivDiv";
                                    let txtbx = document.createElement("input");
                                    txtbx.value = recept.naziv;
                                    div.appendChild(txtbx);
                                    divDodavanje.appendChild(div);


                                    div = document.createElement("div");
                                    div.className = "DodavanjeSastojciDiv";

                                    sastojci.forEach(sastojak => {
                                        sastojak.crtaj(div);

                                    });

                                    let btnDodajStvar = document.createElement("button");
                                    btnDodajStvar.className = "BtnDodajSastojak";
                                    btnDodajStvar.order = 1;
                                    btnDodajStvar.innerHTML = "Dodaj Sastojak"
                                    btnDodajStvar.onclick = (ev) => {
                                        let s = new SastojakMera();
                                        s.crtaj(div);
                                        sastojci.push(s);
                                    };


                                    div.appendChild(btnDodajStvar);
                                    divDodavanje.appendChild(div);


                                    let divKoraci = document.createElement("div");
                                    divKoraci.className = "DodavanjeKoraciDiv";

                                    divDodavanje.appendChild(divKoraci);


                                    koraci.forEach(korak => {
                                        korak.crtaj(divKoraci);
                                    });



                                    btnDodajStvar = document.createElement("button");
                                    btnDodajStvar.className = "BtnDodajKorak";
                                    btnDodajStvar.style.order = 3;
                                    btnDodajStvar.innerHTML = "Dodaj Korak"

                                    btnDodajStvar.onclick = (ev) => {
                                        let korak = new KorakDrawer();
                                        korak.crtaj(divKoraci);
                                        koraci.push(korak);

                                    };

                                    divKoraci.appendChild(btnDodajStvar);
                                    divDodavanje.appendChild(divKoraci);


                                    let newDiv = document.createElement("div");
                                    newDiv.className = "DodavanjeDugmeDiv";

                                    let btnDodajRecept = document.createElement("button");
                                    btnDodajRecept.innerHTML = "IZMENI RECEPT";
                                    btnDodajRecept.className = "BtnDodajRecept";

                                    newDiv.appendChild(btnDodajRecept);
                                    divDodavanje.appendChild(newDiv);

                                    btnDodajRecept.onclick = (ev) => {

                                        sastojci.forEach(sastojak => {
                                            sastojak.loadData();
                                        });

                                        var brKorak = 1;
                                        koraci.forEach(korak => {
                                            brKorak = korak.loadData(brKorak);
                                        });


                                        var naziv = txtbx.value.trim();
                                        if (naziv.length === 0) {
                                            alert("Recept mora imati naziv!");
                                            return;
                                        }

                                        this.tryUpdateRecept(naziv, sastojci, koraci);

                                    };

                                });
                            } else {
                                throw rspns;
                            }
                        }).catch(err => err.text().then(errmsg => alert(errmsg)));

                });
            } else {
                throw response;
            }

        }).catch(err => err.text().then(errmsg => alert(errmsg)));
    }

    oceni(ocena) {

        if (!this.ocenio) {


            fetch(this.fetchBegin + "ReceptOcena/Oceni/" + this.id + "/" + this.ulogovaniKor.id + "/" + ocena, { method: "POST" })
                .then(resp => {
                    if (resp.ok) {
                        alert("Ocenili ste recept!");
                        return;
                    }
                    throw resp;
                }).catch(err => {
                    err.text().then(e => { alert(e) });
                });
        } else {
            fetch(this.fetchBegin + "ReceptOcena/Update/" + this.id + "/" + this.ulogovaniKor.id + "/" + ocena, { method: "PUT" })
                .then(resp => {
                    if (resp.ok) {
                        alert("Ocenili ste recept!");
                        return;
                    }
                    throw resp;
                }).catch(err => {
                    err.text().then(e => { alert(e) });
                });
        }

    }
}