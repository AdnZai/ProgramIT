

class App {
    constructor() {
    
        this.load();
    }

    async load() {
        await htmls.loadHtmls();
       this.elements = {};
        const test = E.load("Aktywacja").prependTo(document.body).loadElementsTo(this.elements).build();


        window.getParameterByName = function(name) {
          name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
          var regexS = "[\\?&]" + name + "=([^&#]*)";
          var regex = new RegExp(regexS);
          var results = regex.exec(window.location.href);
          if (results == null) return "";
          else return decodeURIComponent(results[1].replace(/\+/g, " "));
       }
      
     


     var  kluczAktywacja = getParameterByName("KluczAktywacyjny")




  const url = `${location.origin}/Data/AktywujKonto`

  const postData = { kluczDoAktywacjiKonta: kluczAktywacja};
  const response = await fetch(
          url,
           { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
            
       );
       
       if (!response.ok) {
        alert("Błąd odpowiedzi serwera");
          return;
      }
  
       const result = await response.json();
  
  
       if(result.aktywowane){
  
     
      alert(result.komunikat)
  
    }
    else
    {
      alert(result.komunikat)
    }


    window.close();


    }

  }



 

const app = new App();

