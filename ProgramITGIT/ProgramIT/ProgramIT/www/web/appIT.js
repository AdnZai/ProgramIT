kluczLogowania = "";
idWyborEdycja = '';



class App {
    constructor() {
    
        this.load();
    }
 

    async load() {
        await htmls.loadHtmls();
       this.elements = {};
        const test = E.load("it").prependTo(document.body).loadElementsTo(this.elements).build();
       this.elements.buttonWyloguj.onclick = () => this.buttonClickbuttonWyloguj();

       kluczLogowania = sessionStorage.getItem('kluczLogowania')
       sessionStorage.removeItem('kluczLogowania')

    
      
    }



    async buttonClickbuttonWyloguj()
    {
      kluczLogowania = "";
window.close();


    }



  }









function CreateTableOnDocument (tableToCreate, jakiepoleDodac, kolumnadanedowyboru, gdzieWyswietlic, tableName, nazwyKolumnDoOznaczenia) {
  var liczba;
    var idKolumnDoOznaczenia = ""
    var divContainer = document.getElementById(gdzieWyswietlic);
    divContainer.innerHTML = "";
    
              
   
   if(tableToCreate === "[]" || tableToCreate === null || tableToCreate === undefined || tableToCreate.length === 0){
   
      return;
    }
  
   
  
      var col = [];
    var ilosckolumn = -1;
      for (var i = 0; i < tableToCreate.length; i++) {
                  for (var key in tableToCreate[i]) {
                      if (col.indexOf(key) === -1) {
                          col.push(key); 
                        
                          ilosckolumn++;
                      }
                  }
              }
             if (jakiepoleDodac != ""){
             col.push("Wybór");
              
              }
              ilosckolumn++;
             
    
              var table = document.createElement("table");
              table.classList.add('table');
              table.classList.add('table-striped');
             

              table.id="dtBasicExample"
           
         
              var thead = document.createElement("thead");

      
       var tr2 = thead.insertRow(-1);
              
      
              for (var i = 0; i < col.length; i++) {
                  var th = document.createElement("th");
                  if(nazwyKolumnDoOznaczenia.includes(col[i])){
                    idKolumnDoOznaczenia = idKolumnDoOznaczenia+";"+i+";"
                  }
                  th.innerHTML = col[i];
                  th.style = "position: sticky; top: -1px; background: white;"
                  tr2.appendChild(th);
                 
              }
              thead.appendChild(tr2)
            table.appendChild(thead);

              var tbody = document.createElement("tbody");
              var tr;  
             
              for (var i = 0; i < tableToCreate.length; i++) {
                
                 tr = tbody.insertRow(-1);
  
                 
                for (var j = 0; j < col.length; j++) {
                    var tabCell = tr.insertCell(-1);
                    tabCell.setAttribute("align","center")
                    tabCell.style.color = "black"
  
                    if(j == ilosckolumn){
                if (jakiepoleDodac == "button"){
            
            
                  
                      tabCell.innerHTML = "<button type=\""+ jakiepoleDodac +"\" class=\"btn fa fa-pencil\" name=\""+tableName+"\" value=\' "+ tableToCreate[i][col[kolumnadanedowyboru]] +"'\" onclick=\"WyswietlDaneEdycja(this.value)\"/>"

                   }
                    }
                    else
                    {
               
                      tabCell.innerHTML = tableToCreate[i][col[j]];
                    }
                  
                    
                }
            }
            table.appendChild(tbody);
            
            
  
            divContainer.appendChild(table);
    
    }

    function checkNumber(x) {
      if(typeof x == 'number' && !isNaN(x)){
          if (Number.isInteger(x)) {
            return true;
          }
          else {
              return true;
          }
      
      } else {
          return false;
      }
  }


  







  
  function showInfo(czaswyswietlania, cowyswietlic,jakikomunikat){
    clearTimeout(x);
    jQuery(document).ready(function(){
      $(".alert-success").hide();
    });
    jQuery(document).ready(function(){
      $(".alert-danger").hide();
    });
    jQuery(document).ready(function(){
      $(".alert-warning").hide();
    });
    if(jakikomunikat == 'sukces'){
  
    document.getElementById('idkomunikatsukces').innerHTML = `<p><b>${cowyswietlic}</b></p>`;
     jQuery(document).ready(function(){
       $(".alert-success").show(300);
   });
   x = setTimeout(function(){
     jQuery(document).ready(function(){
       $(".alert-success").hide(300);
     });
     }, czaswyswietlania);
    }
    else if(jakikomunikat == 'denger')
    {
    
      document.getElementById('idkomunikatdanger').innerHTML = `<p><b>${cowyswietlic}</b></p>`;
      jQuery(document).ready(function(){
        $(".alert-danger").show(300);
    });
    x = setTimeout(function(){
      jQuery(document).ready(function(){
        $(".alert-danger").hide(300);
      });
      }, czaswyswietlania);
      
    }
    else if(jakikomunikat == 'warning')
    {
   
      document.getElementById('idkomunikatwarning').innerHTML = `<p><b>${cowyswietlic}</b></p>`;
      jQuery(document).ready(function(){
        $(".alert-warning").show(300);
    });
    x = setTimeout(function(){
      jQuery(document).ready(function(){
        $(".alert-warning").hide(300);
      });
      }, czaswyswietlania);
      
    }
    }


  



function SprawdzCheckCzyZaznaczony(podajIdElementu){
  if (document.getElementById(podajIdElementu).checked){
  return true;
  }
  else
  {
    return false;
  }
    }




    

async function WyswietlDaneEdycja(wartosc){

  WyczyscPolaZamknij();

  spinner.style.visibility = 'visible'
  


idWyborEdycja =  wartosc;
 
if(idWyborEdycja == ''){
spinner.style.visibility = 'hidden'
showInfo(4000,'NIE DOKONANO WYBORU','warning')
return;
}

const url = `${location.origin}/Data/ITEdycja`



const postData = { keyAuth: kluczLogowania, EdycjaDanych: true, idEdycja: idWyborEdycja, CzegoSzukac: ''};
const response = await fetch(
      url,
       { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
        
   );
   
   if (!response.ok) {
    spinner.style.visibility = 'hidden'
    showInfo(3000,'Błąd odpowiedzi serwera. Spróbuj ponownie.','warning');
      return;
  }

   const result = await response.json();


  if(result.zalogowany){
  
  
    
     if(result.grupa == '')WczytajStalePolaCombo('idDropDownGrupa',false,result.grupa)
    else WczytajStalePolaCombo('idDropDownGrupa',true,result.grupa)

    
    
    
    document.getElementById("checkprywatne").checked =  result.status


    document.getElementById('idNazwa').value = result.nazwa 

   document.getElementById('idOpis').value =  result.opis 

  document.getElementById('idHaslo').value =  result.haslo


 
  WyswietlElement('.DodajPozycje')
 




    
    
   
    
    
    



   
   }
   else
   {
    idWyborEdycja = ''
    showInfo(3000,'BŁąd weryfikacji logowania. Zaloguj się ponownie.','denger')
   }

   spinner.style.visibility = 'hidden'
 }

 async function ZmianaHasla(){

  
  let nwepassword = prompt("Podaj nowe hasło", "");

  if (nwepassword == null) {
   return;
  } 

  const url = `${location.origin}/Data/ZmianaHasla`
console.log(kluczLogowania)
const postData = { keyAuth: kluczLogowania, passUzyt: nwepassword};
const response = await fetch(
      url,
       { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
        
   );
   
   if (!response.ok) {
    showInfo(3000,'Błąd odpowiedzi serwera. Spróbuj ponownie.','warning');
      return;
  }

   const result = await response.json();


   if(result.zalogowany){
   
    if(result.bladDanych){
      alert(result.komunikat)
    }
    else
    {
      if(result.czyZmieniono){
      alert('Hasło zostało zmienione')
    }
    else
    {
      alert('Problem ze zmianą')
    }
    }
   }
   else
   {
     alert(result.komunikat);
   }
}


 async function WczytajTabele(jakiepoleDodac, kolumnadanedowyboru)
  {
    WyswietlElement('.WyswietlajDane')
    let spinner = document.getElementById("spinner");
    spinner.style.visibility = 'visible'
    const url = `${location.origin}/Data/ProgITPobierzTabele`

   
    const postData = { keyAuth: kluczLogowania, CzegoSzukac: document.getElementById('idinputSzukaj').value};
    const response = await fetch(
            url,
             { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
              
         );
         
         if (!response.ok) {
          spinner.style.visibility = 'hidden'
            alert("Response Err");
            return;
        }
    
         const result = await response.json();
        
    if(result.zalogowany){
      spinner.style.visibility = 'hidden'
      var tabela = JSON.parse(result.tabela);
      CreateTableOnDocument(tabela, jakiepoleDodac, kolumnadanedowyboru,'showData','Dane','');

    }
    else
    {
      spinner.style.visibility = 'hidden'
    alert(result.komunikat);
  }
  
 



  }







 function ladujDropDown(JsonData, DropDownIdElement, locValue, locName,ustawDomyslnie,coUstawDomyslnie) {

  
  var ele = document.getElementById(DropDownIdElement);
  if(JsonData != '')

  {

  removeOptions(ele);
  if(ustawDomyslnie){
    ele.innerHTML = '<option value="" disabled>Wybierz jedną z opcji</option>'
  }
  else
  {
    
    ele.innerHTML = '<option value="" disabled selected>Wybierz jedną z opcji</option>'
  }
        for (var i = 0; i < JsonData.length; i++) {
          if (ustawDomyslnie && coUstawDomyslnie == JsonData[i][locName])
{

ele.innerHTML = ele.innerHTML + '<option value="' + JsonData[i][locValue] + '" selected>' + JsonData[i][locName] + '</option>';
} 
else
{          

   ele.innerHTML = ele.innerHTML +
                '<option value="' + JsonData[i][locValue] + '">' + JsonData[i][locName] + '</option>';
              
}  
        }
      }
      else
      {
        removeOptions(ele);
        ele.innerHTML = '<option value="" disabled selected>Brak opcji do wyboru, wprowadź ręcznie</option>'
    }
  }

  function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for(i = L; i >= 0; i--) {
       selectElement.remove(i);
    }
 }

 function UkryjElement(coUkryc){
  jQuery(document).ready(function(){
    $(coUkryc).hide(300);});
}
     
function WyswietlElement(coWyswietlic){
  jQuery(document).ready(function(){
    $(coWyswietlic).show(300);});

}


function WyswietlDodOpcje(show1,show2,wyczyscDaneIdPola) {

  

  if(!$(show1).is(':visible')){
  document.getElementById(wyczyscDaneIdPola).value = ''
    jQuery(document).ready(function(){
      $(show2).hide(100);
    });
    jQuery(document).ready(function(){
      $(show1).show(200);
  });
  
  }
  else
  {
  
    jQuery(document).ready(function(){
      $(show2).show(200);
  });
  jQuery(document).ready(function(){
    $(show1).hide(100);
  });
  }
  }

function WyczyscPolaZamknij(){
  idWyborEdycja = '';

  document.getElementById("checkprywatne").checked =  false


  document.getElementById('idNazwa').value = '' 

 document.getElementById('idOpis').value =  ''

document.getElementById('idinputGrupa').value = ''

document.getElementById('idHaslo').value =  ''

}

  async function ZapiszDane( )
{
  

  let spinner = document.getElementById("spinner");
  spinner.style.visibility = 'visible'
  const url = `${location.origin}/Data/ProgramITZapisz`
 
  const postData = { keyAuth: kluczLogowania, CzegoSzukac: document.getElementById('idinputSzukaj').value, strNazwa: document.getElementById('idNazwa').value, strHaslo: document.getElementById('idHaslo').value
  , strOpis: document.getElementById('idOpis').value, strGrupa: document.getElementById('idDropDownGrupa').options[document.getElementById('idDropDownGrupa').selectedIndex].text.trim(), 
  strGrupaDod: document.getElementById('idinputGrupa').value, statusZapisu:  SprawdzCheckCzyZaznaczony('checkprywatne'), idEdycja: idWyborEdycja};
  const response = await fetch(
          url,
           { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
            
       );
       
       if (!response.ok) {
        spinner.style.visibility = 'hidden'
       
          alert("Response Err");
          return;
      }

       const result = await response.json();
       console.log(result.zalogowany);
  if(result.zalogowany){
    spinner.style.visibility = 'hidden'
    if(result.zapisano){
      WyczyscPolaZamknij();
      
      showInfo('3000','ZAPISANO','sukces')

      WczytajTabele('button',0);
      WczytajStalePolaCombo('idDropDownGrupa',false,'')
      idWyborEdycja = '';
  
    }
    else
    {
showInfo('3000','BŁĄD ZAPISU DANYCH','warning')
    }
  }
  else
  {
    spinner.style.visibility = 'hidden'
   
  alert("Bląd");
}


}


async function WczytajStalePolaCombo(gdzieWczytujemy,ustawDomyslnie,coUstawDomyslnie){

        
     
  url = `${location.origin}/Data/WczytajComboBox`



     
    postData = { keyAuth: kluczLogowania};


  
  const response = await fetch(
          url,
           { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
            
       );
       
       if (!response.ok) {
        showInfo(3000,'Błąd odpowiedzi serwera. Spróbuj ponownie.','warning');
          return;
      }
  
       const result = await response.json();


       if(result.zalogowany){
      
        ladujDropDown(JSON.parse(result.tabela),gdzieWczytujemy, 'id','nazwa',ustawDomyslnie,coUstawDomyslnie);
       
      }
      else
      {
        showInfo(2000,result.komunikat,'denger')
      }


}


async function Usun()
  {
    

    let spinner = document.getElementById("spinner");
    spinner.style.visibility = 'visible'
    const url = `${location.origin}/Data/ProgramITUsun`
   
    const postData = { keyAuth: kluczLogowania, idEdycja: idWyborEdycja};
    const response = await fetch(
            url,
             { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(postData) }
              
         );
         
         if (!response.ok) {
          spinner.style.visibility = 'hidden'
            alert("Response Err");
            return;
        }
    
         const result = await response.json();
        
    if(result.zalogowany){
      if(result.zapisano){
      idDimWyborEdycja = '';
      WczytajStalePolaCombo('idDropDownGrupa',false,'')
      WyczyscPolaZamknij();
      UkryjElement('.DodajPozycje')
      WczytajTabele("button",0);
      showInfo('3000','ZAPISANO','sukces')
      
     
      spinner.style.visibility = 'hidden'
    }
    else
    {
      showInfo('3000','BŁĄD. REKORD NIE ZOSTAŁ USUNIĘTY','warning')
      spinner.style.visibility = 'hidden'
    }
    }
    else
    {
      spinner.style.visibility = 'hidden'
    
    alert("Bląd");
  }
  
  spinner.style.visibility = 'hidden'



  }


  function WyswietlOknoUsun(){

    jQuery(document).ready(function(){
      $("#ModalUsunPytanie").modal('show');
    });
  }


const app = new App();

