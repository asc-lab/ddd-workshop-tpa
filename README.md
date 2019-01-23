# ddd-workshop-tpa

## TPA OK - opis domeny

Nasza firma TPA OK zajmuje si� administracj� polisami. Podpisuje umow� z towarzystwem ubezpieczeniowym na obs�ug� szk�d medycznych z polis sprzedawanych przez towarzystwo.

Towarzystwa dostarczaj� nam dane polis oraz zmian na polisach w postaci plik�w, kt�re przesy�ane s� do nas z r�n� cz�stotliwo�ci�. Niekt�re towarzystwa wysy�aj� plik raz dziennie, inne w cyklach tygodniowych.
Staramy si�, negocjuj�c umowy zapewni�, �e towarzystwo przeka�e nam dane w ustalonym przez nas wsp�lnym formacie.

Poni�ej przyk�ad fragmentu takiego pliku zawieraj�cego jedn� polis�:

```json
{
	"policyNumber": "P1212121",
	"Insurer": "PZYOU",
	"productCode": "Pakiet Gold",
	"Insureds": [ {
		"firstName": "Jan",
		"lastName": "Nowak",
		"pesel": "1111111116",
		"accountNumber": "2738123834783247723",
		"address": {
			"country": "PL",
			"city": "Warszawa",
			"zipCode": "01-001",
			"street": "JaksTam 123 m 2"
		}
	}, {
		"firstName": "Maria",
		"lastName": "Nowak",
		"pesel": "2111111116",
		"accountNumber": "2738123834783247723",
		"address": {
			"country": "PL",
			"city": "Warszawa",
			"zipCode": "01-001",
			"street": "JaksTam 123 m 2"
		}
	}  
],
	"policyValidFrom": "2018-01-01",
	"policyValidTo": "2018-12-31",
	"changeDate": "2018-01-01",
	"covers": [{
			"coverCode": "KONS",
			"services": [{
					"code": "KONS_INTERNISTA",
					"coPayment": {
						"percent": 0.25
					},
					"limit": {
						"maxQuantity": null,
						"maxAmount": 100,
						"limitPeriod": "POLICY_YEAR"
						�Shared� : true
 					}
				}, {
					"code": "KONS_PEDIATRA",
					"coPayment": {
						"amount": 10
					},
					"limit": {
						"maxQuantity": 20,
						"maxAmount": 100,
						"limitPeriod": "POLICY_YEAR"
					}
				}
			]
		}, {
			"coverCode": "LAB",
			"services": [{
					"code": "LAB_KREW_OB",
					"coPayment": {
						"percent": 0.10
					},
					"limit": {
						"maxQuantity": 5,
						"maxAmount": 50,
						"limitPeriod": "POLICY_YEAR"
					}
				}, {
					"code": "LAB_HDL",
					"coPayment": {
						"amount": 2
					},
					"limit": {
						"maxQuantity": 2,
						"maxAmount": 28,
						"limitPeriod": "POLICY_YEAR"
					}
				}
			]
		}
	]
}
```

Otrzymujemy numer polisy, kod ubezpieczyciela, dane ubezpieczonego, okres obowi�zywania polisy, dat� zmiany (dla nowej polisy powinna zawsze by� r�wna dacie pocz�tku obowi�zywania), kod produktu oraz list� ochron. Ochrony okre�laj� jakie us�ugi medyczne s� obj�te polis�.
Dla ka�dej us�ugi mo�e by� okre�lona wsp�p�atno�� (procentowa lub kwotowa) oraz limit. Wsp�p�atno�� to konkretna kwota lub procent ceny us�ugi, jak� pacjent musi zap�aci� sam przy ka�dej us�udze. Limit oznacza ilo�� us�ug / koszt jaki pokrywa ubezpieczyciel. Limit ma okre�lony czas po kt�rym jest odnawiany.
W chwili obecnej obs�ugujemy limity na rok polisowy, na szkod� i rok kalendarzowy.  
Limit mo�e dotyczy� pojedynczego ubezpieczonego b�d� by� wsp�lny dla wszystkich os�b na polisie.

Nasza firma podpisuje umowy z plac�wkami medycznymi i sieciami takich plac�wek na �wiadczenie us�ug medycznych. Umowa taka zawierana jest na czas okre�lony (zazwyczaj na rok). Umowa zawiera list� plac�wek oraz cennik us�ug. Ceny mog� si� zmienia� w trakcie obowi�zywania umowy.
Niekt�re plac�wki mog� mie� wsp�lne cenniki, na przyk�ad wszystkie plac�wki MedHelp w Warszawie maj� wsp�lny cennik. Rozliczenia z plac�wkami odbywaj� si� wed�ug tych cennik�w. 

Pacjenci zg�aszaj� si� do plac�wek lub dzwoni� na nasze call center. Musimy teraz dokona� autoryzacji wizyty osoby w plac�wce medycznej. W tym celu musimy wyszuka� dan� osob� i sprawdzi� czy ma ona aktywn� polis�. Je�li nie mamy danych takiej osoby lub nie ma ona aktywnej polisy to informujemy o tym i nie obs�ugujemy dalej. Nast�pnie w przypadku call center musimy wyszuka� plac�wk�. Dopuszczamy sytuacj�, �e wizyta odbywa si� w plac�wce, z kt�r� nie mamy umowy. Rejestrujemy spraw� i zbieramy informacj� o planowanych us�ugach: kod us�ugi, cena i ilo�� us�ug. Maj�c te informacje mo�emy znaj�c histori� ubezpieczenia obliczy� ile powinien zap�aci� pacjent, ile powinien pokry� ubezpieczyciel. U�ytkownik z odpowiednimi kompetencjami mo�e dokona� manualnej korekty takiej kalkulacji, w tym odm�wi� pokrycia kosztu us�ugi. Musi on w takiej sytuacji poda� pow�d swojej decyzji. W przypadku plac�wek, z kt�rymi mamy podpisan� umow� cena nie mo�e przekracza� ceny ustalonej w cenniku. Taka wizyta mo�e teraz zosta� zaakceptowana lub odrzucona przez pacjenta. 
Po wykonaniu zabiegu plac�wka potwierdza (chcieliby�my mie� to w systemie) wykonanie lub pacjent przysy�a do nas faktur� za wizyt� w przypadku plac�wek z kt�rymi nie mamy umowy). W przypadku faktury musimy wyszuka� spraw� i sprawdzi� czy koszt zgodny jest z naszymi wyliczeniami. Je�li nie, musimy spraw� wyja�ni� z plac�wk� medyczn� i dokona� korekty. 
Plac�wki, z kt�rymi mamy podpisane umowy przesy�aj� nam raporty wykonanych us�ug (w cyklach tygodniowych lub miesi�cznych). Taki raport musi zosta� zweryfikowany ze stanem po naszej stronie. Report zawiera kody us�ug, daty realizacji, cen�, dane identyfikuj�ce pacjenta i numer polisy. Us�ugi, kt�re s� u nas zarejestrowane a nie ma ich w raporcie uwa�amy za niewykonane. Us�ugi, kt�rych nie uda�o si� zidentyfikowa� s� tworzone i oznaczane jako wymagaj�ce wyja�nienia. Us�ugi zidentyfikowane, gdzie wyst�puj� r�nice r�wnie� oznaczane s� jako do wyja�nienia.
Przegl�damy takie us�ugi i po wyja�nieniu z plac�wk� / pacjentem akceptujemy lub odrzucamy.

Klient mo�e r�wnie� do nas zadzwoni�, �eby sprawdzi� czy dany zabieg lub badanie jest pokrywany przez jego ubezpieczenie.

Rozliczenie z plac�wkami medycznymi odbywa si� w cyklach okre�lonych na umowie (miesi�cznie lub kwartalnie). Za ka�d� zrealizowan� us�ug� przekazujemy kwot� wyliczon� wcze�niej jako udzia� ubezpieczyciela. P�atno�� odbywa si� na podstawie faktury przekazanej przez plac�wk�. 

Z ubezpieczycielami rozliczamy si� r�wnie� w cyklach okre�lonych na umowie. Wystawimy faktur� zawieraj�ca koszt wyliczony jako udzia� ubezpieczyciela we wszystkich sprawach w danym okresie. Faktura rozbita jest na pozycje per produkt ubezpieczyciela i us�uga. Do faktury doliczamy rycza�t za ka�dego ubezpieczonego zgodnie z stawk� na umowie.

Poniewa� kontrola koszt�w jest dla i naszych klient�w niezwykle wa�na, to chcieliby�my mie� mo�liwo�� wykrywania nadu�y� np. du�a cz�stotliwo�� korzystania z danej us�ugi lub dziwne po��czenie diagnozy z us�ug�.


Spos�b wyliczenia podzia�u kosztu us�ugi:
sprawdzenie, czy na polisie w ochronach jest us�uga o kodzie z pozycji, je�li nie ma to ca�y koszt ponosi ubezpieczony,
system wylicza udzia� w�asny na podstawie definicji co-payment z polisy (procentowy lub kwotowy)
nast�pnie pozosta�a kwota por�wnywana jest z limitem na dan� us�ug� i dotychczasowym zu�yciem tego limitu

Przyk�ad limitu:  "limit" : max koszt 100 PLN, okres rozliczenia rok polisowy oznacza, �e w ci�gu roku polisowego ubezpieczonemu przys�uguje zwrot maksymalnie 100 PLN, ale ma nieograniczon� liczb� wyst�pienia us�ugi. 

W ten spos�b powstaje kwota do zap�aty przez ubezpieczyciela.
