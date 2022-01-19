import { Korak } from "./Korak.js";
import { KorakDrawer } from "./KorakDrawer.js";
import { Korisnik } from "./Korisnik.js";
import { Recept } from "./Recept.js";
import { Sastojak } from "./Sastojak.js";
import { SastojakMera } from "./SastojakMera.js";

export class Kuvar {


    constructor(id, naziv, fetchBegin) {
        this.id = id;
        this.naziv = naziv;
        this.container = null;
        this.korisnik = null;

        this.fetchBegin = fetchBegin;
    }


    crtajPocetak(host) {

        if (this.container === null) {
            this.container = document.createElement("div");;
            host.appendChild(this.container);

        }
        this.crtajHeaderZaLogin(this.container);

        this.crtajLogoISearchBar(this.container);


    }

    crtajHeaderZaLogin(host) {
        let div = document.createElement("div");
        div.className = "logingDiv";
        div.classList.add("mainScreen");

        let l = document.createElement("label");
        if (this.korisnik === null) {
            l.className = "logLbl";
            l.innerHTML = "Registruj se";
            l.addEventListener("click", ev => {
                this.registrujSe();
            })
            div.appendChild(l);

            l = document.createElement("label");
            l.className = "logLbl";
            l.innerHTML = "Uloguj se";
            l.addEventListener("click", ev => {
                this.cls();
                this.ulogujSe();
            })
            div.appendChild(l);
        } else {

            l.className = "LogoutLbl";
            l.innerHTML = "Izloguj se";
            l.addEventListener("click", ev => {
                this.cls();
                this.korisnik = null;

                var chkbxs = this.container.querySelector(".checkboxDiv");
                if (chkbxs !== null)
                    this.container.removeChild(chkbxs);

                var divs = this.container.querySelectorAll(".mainScreen");
                divs.forEach(element => {
                    this.container.removeChild(element);
                });

                this.crtajPocetak(this.container);

            });

            div.appendChild(l);

            l = document.createElement("label");
            l.className = "DodajReceptLbl";
            l.innerHTML = "Dodaj recept";
            l.onclick = (ev) => {
                this.cls();
                this.crtajDodajRecept();
            };
            div.appendChild(l);

            l = document.createElement("label");
            l.className = "MyReceptiLbl";
            l.innerHTML = "Moji recepti";
            l.onclick = (ev) => {
                this.cls();
                this.crtajMojeRecepte();
            };
            div.appendChild(l);


            l = document.createElement("label");
            l.className = "NameLbl";
            l.innerHTML = this.korisnik.ime + " " + this.korisnik.prezime + " : ";
            div.appendChild(l);
        }

        host.appendChild(div);
    }


    odustani() {
        var divs = this.container.querySelectorAll(".RegDiv");
        divs.forEach(element => {
            this.container.removeChild(element);
        });
        this.crtajPocetak(this.container);
    }

    registrujSe() {
        this.cls();

        var chkbxs = this.container.querySelector(".checkboxDiv");
        if (chkbxs !== null)
            this.container.removeChild(chkbxs);

        var divs = this.container.querySelectorAll(".mainScreen");
        divs.forEach(element => {
            this.container.removeChild(element);
        });


        let div = document.createElement("div");
        div.className = "ExitRegDiv";
        div.classList.add("RegDiv");

        let lbl = document.createElement("label");
        lbl.innerHTML = "Odustani";
        lbl.addEventListener("click", ev => {
            this.odustani();
        });

        div.appendChild(lbl);
        this.container.appendChild(div);

        div = document.createElement("div");
        div.className = "RegistracijaDiv";
        div.classList.add("RegDiv");
        this.container.appendChild(div);


        lbl = document.createElement("label");
        lbl.innerHTML = "Registruj se";
        div.appendChild(lbl);


        let txtbx = document.createElement("input");
        txtbx.className = "imeTxt";
        txtbx.placeholder = "Ime";

        div.appendChild(txtbx);

        txtbx = document.createElement("input");
        txtbx.className = "prezimeTxt";
        txtbx.placeholder = "Prezme";

        div.appendChild(txtbx);

        txtbx = document.createElement("input");
        txtbx.className = "emailTxt";
        txtbx.placeholder = "E-mail";

        div.appendChild(txtbx);

        txtbx = document.createElement("input");
        txtbx.type = "password";
        txtbx.className = "passTxt";
        txtbx.placeholder = "Lozinka";

        div.appendChild(txtbx);

        let btn = document.createElement("button");
        btn.innerHTML = "Registruj se";
        btn.className = "RegBtn";
        btn.addEventListener("click", ev => {
            this.doRegister(div);
        });

        div.appendChild(btn);

    }


    ulogujSe() {
        var chkbxs = this.container.querySelector(".checkboxDiv");
        if (chkbxs !== null)
            this.container.removeChild(chkbxs);

        var divs = this.container.querySelectorAll(".mainScreen");
        divs.forEach(element => {
            this.container.removeChild(element);
        });

        let div = document.createElement("div");
        div.className = "ExitRegDiv";
        div.classList.add("RegDiv");

        let lbl = document.createElement("label");
        lbl.innerHTML = "Odustani";
        lbl.addEventListener("click", ev => {
            this.odustani();
        });

        div.appendChild(lbl);
        this.container.appendChild(div);

        div = document.createElement("div");
        div.className = "RegistracijaDiv";
        div.classList.add("RegDiv");
        this.container.appendChild(div);


        lbl = document.createElement("label");
        lbl.innerHTML = "Uloguj se";
        div.appendChild(lbl);



        var txtbx = document.createElement("input");
        txtbx.className = "emailTxt";
        txtbx.placeholder = "E-mail";

        div.appendChild(txtbx);

        txtbx = document.createElement("input");
        txtbx.type = "password";
        txtbx.className = "passTxt";
        txtbx.placeholder = "Lozinka";

        div.appendChild(txtbx);

        let btn = document.createElement("button");
        btn.innerHTML = "Uloguj se";
        btn.className = "RegBtn";
        btn.addEventListener("click", ev => {
            this.doLogin(div);
        });

        div.appendChild(btn);
        this.container.appendChild(div);

    }


    doRegister(host) {
        var ime = host.querySelector(".imeTxt").value.trim();
        var preziime = host.querySelector(".prezimeTxt").value.trim();
        var email = host.querySelector(".emailTxt").value.trim();
        var sifra = host.querySelector(".passTxt").value.trim();

        if (ime === "" || preziime === "" || email === "" || sifra === "") {
            alert("Morate uneti sve podatke da biste se registrovali!");
            return;
        }

        const passRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#\$\^\+=!\*\(\)@%&]).{8,}$/;
        if (!sifra.match(passRegex)) {
            alert("Lozinka mora sadrzati barem jedno malo slovo, jedno veliko slovo, jedan poseban znak i mora biti dugacka barem 8 karaktera!");
            return;
        }

        const emailRegex = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (!email.match(emailRegex)) {
            alert("Email loseg oblika!");
            return;
        }

        fetch(this.fetchBegin + "Korisnik/DodajKorisnika/", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ime: ime,
                prezime: preziime,
                email: email,
                password: sifra
            })
        }).then(resp => {
            if (resp.ok) {

                alert("Korisnik dodat");
                resp.json().then(r => {
                    this.korisnik = new Korisnik(r.id, r.ime, r.prezime);
                    this.odustani();
                });
            } else {
                throw resp;
            }
        }).catch(error => {
            error.text().then(errorMsg => {
                alert(errorMsg);
            });
        });
    }

    doLogin(host) {
        var email = host.querySelector(".emailTxt").value.trim();
        var sifra = host.querySelector(".passTxt").value.trim();

        if (email === "" || sifra === "") {
            alert("Unesite sve podatke!");
            return;
        }

        const emailRegex = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (!email.match(emailRegex)) {
            alert("E-mail loseg oblika!");
            return;
        }
        var link = this.fetchBegin + "Korisnik/Login/";
        var encodedLink = link + encodeURIComponent(email) + "/" + encodeURIComponent(sifra);
        fetch(encodedLink, { method: "GET" })
            .then(resp => {
                if (resp.ok) {
                    resp.json().then(r => {
                        this.korisnik = new Korisnik(r.id, r.ime, r.prezime);
                        this.odustani();
                    });
                } else {
                    throw resp;
                }
            }).catch(error => {
                error.text().then(errorMsg => {
                    alert(errorMsg);
                });
            });
    }


    crtajLogoISearchBar(host) {
        let searchDiv = document.createElement("div");
        searchDiv.className = "searchDiv";
        searchDiv.classList.add("mainScreen");

        let img = document.createElement("img");
        img.src = "kuvar" + this.id + ".png";
        img.className = "logoImg"
        searchDiv.appendChild(img);


        let div = document.createElement("div");
        div.className = "pretragaDiv";

        let serchBar = document.createElement("input");
        serchBar.type = "text";
        serchBar.className = "searchBar";
        serchBar.placeholder = "Pretrazi recept";
        serchBar.addEventListener("keyup", ev => {
            var key = ev.key || ev.keyCode;
            if (key === "Enter") {
                this.cls();
                this.pretraziRecepte(serchBar.value);
                serchBar.value = "";
            }
        });
        div.appendChild(serchBar);

        let btn = document.createElement("button");
        btn.innerHTML = "Pretrazi";
        btn.addEventListener("click", ev => {
            this.cls();
            if (div.querySelector(".searchBar") !== null) {
                this.pretraziRecepte(serchBar.value);
                serchBar.value = "";
            } else {
                var chbxs = this.container.querySelectorAll("input[name=sastojakChkBx]:checked");
                var chbxsIds = [];
                chbxs.forEach(el => {
                    chbxsIds.push(el.value);
                });
                this.pretraziReceptePoSastojcima(chbxsIds);
            }
        });
        div.appendChild(btn);

        btn = document.createElement("button");
        btn.innerHTML = "Pretraga po sastojcima";
        btn.style.marginLeft = "15px";

        btn.addEventListener("click", ev => {
            serchBar.value = "";

            let tabl = this.container.querySelector(".TableDiv");
            if (tabl !== null)
                this.container.removeChild(tabl);
            var chkbxs = this.container.querySelector(".checkboxDiv");
            if (chkbxs === null)
                this.makeActionPretraga(div, btn);
            else
                this.actionOtkaziPretraga(div, btn, serchBar);
        });

        div.appendChild(btn);

        searchDiv.appendChild(div);


        host.appendChild(searchDiv);
    }

    pretraziRecepte(naziv) {
        if (naziv.trim() === "") {
            alert("Morate uneti neki tekst da bi mogli da pretrazujete!");
            return;
        }

        fetch(this.fetchBegin + "Recept/ReceptiKuvara/" + this.id + "/" + naziv, { method: "GET" })
            .then(resp => {
                if (resp.ok) {

                    resp.json().then(recepti => {

                        let div = document.querySelector(".TableDiv");
                        if (recepti.length === 0) {
                            alert("Nije pronadjen nijedan recept!");
                            if (div != null)
                                this.container.removeChild(div);
                            return;
                        }
                        this.crtajReceptTabelu(div, recepti, false);
                    });
                }
            });
    }

    pretraziReceptePoSastojcima(sastojciIDs) {
        if (sastojciIDs.length === 0) {
            alert("Morate da obelezite neki sastojak!");
            return;
        }
        fetch(this.fetchBegin + "ReceptSastojak/PoSastojcima/" + this.id, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(sastojciIDs)

        }).then(resp => {
            if (resp.ok) {

                resp.json().then(recepti => {

                    let div = document.querySelector(".TableDiv");;
                    if (recepti.length === 0) {
                        alert("Nije pronadjen nijedan recept!");
                        if (div != null)
                            this.container.removeChild(div);
                        return;
                    }

                    this.crtajReceptTabelu(div, recepti, false);

                });
            }
        });
    }

    crtajReceptTabelu(div, recepti, isMyRecepti) {
        let tabela;

        if (div === null) {
            div = document.createElement("div");
            div.className = "TableDiv";
            div.classList.add("mainScreen");

            this.container.appendChild(div);

        } else {
            tabela = div.querySelector("table");
        }

        if (tabela == null) {
            tabela = document.createElement("table");
            div.appendChild(tabela);

            var tableHead = document.createElement("thead");
            tabela.appendChild(tableHead);

            var tr = document.createElement("tr");
            tableHead.appendChild(tr);

            let th;
            var zag = ["Naziv", "Ocena", "Korisnik", "", ""];
            if (!isMyRecepti)
                zag.pop();
            zag.forEach(el => {
                th = document.createElement("th");
                th.innerHTML = el;
                tr.appendChild(th);
            });

        } else {
            if (!isMyRecepti) {
                var trs = tabela.querySelector("thead").querySelector("tr").querySelectorAll("th");
                if (trs.length == 5)
                    tabela.querySelector("thead").querySelector("tr").removeChild(trs[4]);
            }
        }

        var tabelaBody = this.removeTableBody(tabela);
        var r;
        var btn;
        recepti.forEach(recept => {
            r = new Recept(recept.id, recept.naziv, recept.korisnik, recept.ocena, this.korisnik, this.fetchBegin);
            r.crtajReceptOpis(tabelaBody, isMyRecepti);
        });

    }

    crtajMojeRecepte() {
        fetch(this.fetchBegin + "Recept/ReceptiKorisnika/" + this.korisnik.id + "/" + this.id).then(resp => {
            if (resp.ok) {

                resp.json().then(recepti => {

                    let div = document.querySelector(".TableDiv");;
                    if (recepti.length === 0) {
                        alert("Nije pronadjen nijedan recept!");
                        if (div != null)
                            this.container.removeChild(div);
                        return;
                    }

                    this.crtajReceptTabelu(div, recepti, true);

                });
            }
        });
    }

    crtajDodajRecept() {

        var sastojci = [];
        var koraci = [];

        let divDodavanje = document.createElement("div");
        divDodavanje.className = "DodavanjeDiv";
        this.container.appendChild(divDodavanje);

        let div = document.createElement("div");
        div.className = "DodavanjeNazivDiv";
        let txtbx = document.createElement("input");
        txtbx.placeholder = "Naziv"
        div.appendChild(txtbx);

        divDodavanje.appendChild(div);

        div = document.createElement("div");
        div.className = "DodavanjeSastojciDiv";

        var s = new SastojakMera();
        s.crtaj(div);
        sastojci.push(s);

        let btnDodajStvar = document.createElement("button");
        btnDodajStvar.className = "BtnDodajSastojak";
        btnDodajStvar.order = 1;
        btnDodajStvar.innerHTML = "Dodaj Sastojak"
        btnDodajStvar.onclick = (ev) => {
            s = new SastojakMera();
            s.crtaj(div);
            sastojci.push(s);
        };


        div.appendChild(btnDodajStvar);
        divDodavanje.appendChild(div);


        let divKoraci = document.createElement("div");
        divKoraci.className = "DodavanjeKoraciDiv";

        divDodavanje.appendChild(divKoraci);


        var korak = new KorakDrawer();
        korak.crtaj(divKoraci);
        koraci.push(korak);

        btnDodajStvar = document.createElement("button");
        btnDodajStvar.className = "BtnDodajKorak";
        btnDodajStvar.style.order = 3;
        btnDodajStvar.innerHTML = "Dodaj Korak"
        btnDodajStvar.onclick = (ev) => {
            korak = new KorakDrawer();
            koraci.push(korak);
            korak.crtaj(divKoraci);
        };

        divKoraci.appendChild(btnDodajStvar);
        divDodavanje.appendChild(divKoraci);


        let newDiv = document.createElement("div");
        newDiv.className = "DodavanjeDugmeDiv";

        let btnDodajRecept = document.createElement("button");
        btnDodajRecept.innerHTML = "DODAJ RECEPT";
        btnDodajRecept.className = "BtnDodajRecept";


        btnDodajRecept.onclick = (ev) => {

            sastojci.forEach(sastojak => {
                sastojak.loadData();
            });

            var brKorak = 1;
            koraci.forEach(korak => {
                brKorak = korak.loadData(brKorak);
            });

            koraci = koraci.filter(e => e.deleted == false);
            sastojci = sastojci.filter(e => e.deleted == false);
            var naziv = txtbx.value.trim();
            if (naziv.length === 0) {
                alert("Recept mora imati naziv!");
                return;
            }

            if (sastojci.length === 0) {
                alert("Morate uneti sastojke za recept!");
                return;
            }
            if (koraci.length === 0) {
                alert("Recept mora imati barem jedan korak!");
                return;
            }

            this.tryAddRecept(naziv, sastojci, koraci);

        };


        newDiv.appendChild(btnDodajRecept);
        divDodavanje.appendChild(newDiv);

    }


    tryAddRecept(naziv, sastojci, koraci) {
        var sast = [];
        var kor = [];
        sastojci.forEach(element => {
            sast.push(new Sastojak(element.naziv, element.mera));
        });
        koraci.forEach(element => {
            kor.push(new Korak(element.brKorak, element.opis));
        });
        var body = JSON.stringify({
            naziv: naziv,
            korisnikID: this.korisnik.id,
            kuvarID: this.id,
            sastojci: JSON.stringify(sast),
            koraci: JSON.stringify(kor)
        }).replace(/\\/g, '').replace(/\"\[/g, '[').replace(/\]\"/g, ']');

        fetch(this.fetchBegin + "Recept/Dodaj", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: body
        }).then(resp => {
            if (resp.ok) {
                alert("Recept dodat!");
            } else {
                throw resp;
            }
        }).catch(err => {
            err.text().then(errMsg => alert(errMsg));
        })

    }

    removeTableBody(tab) {
        var telotab = tab.querySelector(".TabelaRecepti");
        var rod;
        if (telotab !== null) {
            rod = telotab.parentNode;
            rod.removeChild(telotab);
        } else {
            rod = tab
        }
        telotab = document.createElement("tbody");
        telotab.className = "TabelaRecepti";
        rod.appendChild(telotab);
        return telotab;
    }

    makeActionPretraga(host, btn) {
        var searchBar = host.querySelector(".searchBar");
        host.removeChild(searchBar);
        this.crtajCheckboxove(this.container);
        btn.innerHTML = "Otkazi";
    }


    cls() {
        var divs = document.body.querySelectorAll(".ReceptShow");
        divs.forEach(element => {
            this.container.removeChild(element);
        });
        var tableDiv = this.container.querySelector(".TableDiv");
        if (tableDiv !== null) {
            this.container.removeChild(tableDiv);
        }

        divs = this.container.querySelector(".DodavanjeDiv");
        if (divs !== null)
            this.container.removeChild(divs);
    }

    actionOtkaziPretraga(host, btn, searchBar) {
        var chkbxs = this.container.querySelector(".checkboxDiv");

        this.container.removeChild(chkbxs);
        let first = host.querySelector("button");
        host.insertBefore(searchBar, first);

        btn.innerHTML = "Pretraga po sastojcima";
    }

    crtajCheckboxove(host) {
        this.cls();
        let div = document.createElement("div");
        div.className = "checkboxDiv";
        let chk;
        let l;
        fetch(this.fetchBegin + "Sastojak/All/" + this.id, { method: "GET" })
            .then(resp => {
                if (resp.ok) {
                    resp.json().then(sastojci => {

                        sastojci.forEach(sastojak => {
                            chk = document.createElement("input");
                            chk.type = "checkbox";
                            chk.name = "sastojakChkBx";
                            chk.value = sastojak.id;

                            l = document.createElement("label");
                            l.className = "chkbxlbl";
                            l.innerHTML = sastojak.naziv;

                            div.appendChild(chk);
                            div.appendChild(l);

                        });

                    });
                    host.appendChild(div);
                }
            });

    }

}