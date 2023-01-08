kluczLogowania = "";

class App {
    constructor() {
    
        this.load();
    }

    async load() {
        await htmls.loadHtmls();
       this.elements = {};
        const test = E.load("logowanie").prependTo(document.body).loadElementsTo(this.elements).build();
       this.elements.buttonZaloguj.onclick = () => this.buttonClick();
     this.elements.buttonRejestracja.onclick = () => this.buttonClickbuttonRejestracja();
     this.elements.ResetPass.onclick = () =>  this.buttonClickResetPass();
     this.elements.RejUzyt.onclick = () =>  this.buttonClickWyslijRejestracje();

    }


   





 async buttonClickResetPass(){

  const url = `${location.origin}/Data/ResetujHaslo`

  const postData = {mail: this.elements.inputMailReset.value};
  const response = await fetch(
          url,
           { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
            
       );
       
       if (!response.ok) {
        alert("Błąd odpowiedzi serwera");
          return;
      }
  
       const result = await response.json();
  
  
       if(result.czyReset){
  
       this.elements.inputMailReset.value = '';
       alert(result.komunikat)
    }
    else
    {
      alert(result.komunikat)
    }
  }

 




 async buttonClickWyslijRejestracje(){

  const url = `${location.origin}/Data/RejestrujUzytkownika`

  const postData = {strlogin: this.elements.inputLoginCreate.value, strhaslo: this.elements.inputPasswordCreate.value, imie: this.elements.inputImieCreate.value, nazwisko: this.elements.inputNazwiskoCreate.value, mail: this.elements.inputMailCreate.value};
  const response = await fetch(
          url,
           { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
            
       );
       
       if (!response.ok) {
        alert("Błąd odpowiedzi serwera");
          return;
      }
  
       const result = await response.json();
  
  
       if(result.rejestracja){
        this.elements.inputLoginCreate.value = '';
        this.elements.inputPasswordCreate.value = '';
        this.elements.inputImieCreate.value = '';
        this.elements.inputNazwiskoCreate.value = '';
        this.elements.inputMailCreate.value = '';

      alert("Potwierdź rejestrację linkiem wysłanym na adres e-mail")
    
    }
    else
    {
      alert(result.komunikat)
    }
 }



    async buttonClick()
    {

        const url = `${location.origin}/Data/LoginWeryfikacja`;
        const postData = { strlogin: this.elements.inputLogin.value, strhaslo: this.elements.inputPassword.value};
 
        const response = await fetch(
            url,
            { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) });

        if (!response.ok)
       {
     
            alert("Bląd");
            return;
      }
else
{
      const result = await response.json();

if (result.zalogowany == true){

    kluczLogowania = result.keyLogin;
    this.elements.inputPassword.value = "";
    this.elements.inputLogin.value = "";
    const url = `${location.origin}/IndexIT`;
    sessionStorage.setItem('kluczLogowania', kluczLogowania)
    kluczLogowania = '';
    window.open(url);
    sessionStorage.removeItem('kluczLogowania')
  
}
else
{
    alert(result.komunikat);  
}
 
    

}
    }


  



  }

  function goZaloguj() {
   
    if(event.key === 'Enter') {
      document.getElementById("myBtnZaloguj").click();     
    }
}





const app = new App();

