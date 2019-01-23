# ddd-workshop-tpa

## TPA OK - opis domeny

Nasza firma TPA OK zajmuje siê administracj¹ polisami. Podpisuje umowê z towarzystwem ubezpieczeniowym na obs³ugê szkód medycznych z polis sprzedawanych przez towarzystwo.

Towarzystwa dostarczaj¹ nam dane polis oraz zmian na polisach w postaci plików, które przesy³ane s¹ do nas z ró¿n¹ czêstotliwoœci¹. Niektóre towarzystwa wysy³aj¹ plik raz dziennie, inne w cyklach tygodniowych.
Staramy siê, negocjuj¹c umowy zapewniæ, ¿e towarzystwo przeka¿e nam dane w ustalonym przez nas wspólnym formacie.

Poni¿ej przyk³ad fragmentu takiego pliku zawieraj¹cego jedn¹ polisê:

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
						“Shared” : true
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

Otrzymujemy numer polisy, kod ubezpieczyciela, dane ubezpieczonego, okres obowi¹zywania polisy, datê zmiany (dla nowej polisy powinna zawsze byæ równa dacie pocz¹tku obowi¹zywania), kod produktu oraz listê ochron. Ochrony okreœlaj¹ jakie us³ugi medyczne s¹ objête polis¹.
Dla ka¿dej us³ugi mo¿e byæ okreœlona wspó³p³atnoœæ (procentowa lub kwotowa) oraz limit. Wspó³p³atnoœæ to konkretna kwota lub procent ceny us³ugi, jak¹ pacjent musi zap³aciæ sam przy ka¿dej us³udze. Limit oznacza iloœæ us³ug / koszt jaki pokrywa ubezpieczyciel. Limit ma okreœlony czas po którym jest odnawiany.
W chwili obecnej obs³ugujemy limity na rok polisowy, na szkodê i rok kalendarzowy.  
Limit mo¿e dotyczyæ pojedynczego ubezpieczonego b¹dŸ byæ wspólny dla wszystkich osób na polisie.

Nasza firma podpisuje umowy z placówkami medycznymi i sieciami takich placówek na œwiadczenie us³ug medycznych. Umowa taka zawierana jest na czas okreœlony (zazwyczaj na rok). Umowa zawiera listê placówek oraz cennik us³ug. Ceny mog¹ siê zmieniaæ w trakcie obowi¹zywania umowy.
Niektóre placówki mog¹ mieæ wspólne cenniki, na przyk³ad wszystkie placówki MedHelp w Warszawie maj¹ wspólny cennik. Rozliczenia z placówkami odbywaj¹ siê wed³ug tych cenników. 

Pacjenci zg³aszaj¹ siê do placówek lub dzwoni¹ na nasze call center. Musimy teraz dokonaæ autoryzacji wizyty osoby w placówce medycznej. W tym celu musimy wyszukaæ dan¹ osobê i sprawdziæ czy ma ona aktywn¹ polisê. Jeœli nie mamy danych takiej osoby lub nie ma ona aktywnej polisy to informujemy o tym i nie obs³ugujemy dalej. Nastêpnie w przypadku call center musimy wyszukaæ placówkê. Dopuszczamy sytuacjê, ¿e wizyta odbywa siê w placówce, z któr¹ nie mamy umowy. Rejestrujemy sprawê i zbieramy informacjê o planowanych us³ugach: kod us³ugi, cena i iloœæ us³ug. Maj¹c te informacje mo¿emy znaj¹c historiê ubezpieczenia obliczyæ ile powinien zap³aciæ pacjent, ile powinien pokryæ ubezpieczyciel. U¿ytkownik z odpowiednimi kompetencjami mo¿e dokonaæ manualnej korekty takiej kalkulacji, w tym odmówiæ pokrycia kosztu us³ugi. Musi on w takiej sytuacji podaæ powód swojej decyzji. W przypadku placówek, z którymi mamy podpisan¹ umowê cena nie mo¿e przekraczaæ ceny ustalonej w cenniku. Taka wizyta mo¿e teraz zostaæ zaakceptowana lub odrzucona przez pacjenta. 
Po wykonaniu zabiegu placówka potwierdza (chcielibyœmy mieæ to w systemie) wykonanie lub pacjent przysy³a do nas fakturê za wizytê w przypadku placówek z którymi nie mamy umowy). W przypadku faktury musimy wyszukaæ sprawê i sprawdziæ czy koszt zgodny jest z naszymi wyliczeniami. Jeœli nie, musimy sprawê wyjaœniæ z placówk¹ medyczn¹ i dokonaæ korekty. 
Placówki, z którymi mamy podpisane umowy przesy³aj¹ nam raporty wykonanych us³ug (w cyklach tygodniowych lub miesiêcznych). Taki raport musi zostaæ zweryfikowany ze stanem po naszej stronie. Report zawiera kody us³ug, daty realizacji, cenê, dane identyfikuj¹ce pacjenta i numer polisy. Us³ugi, które s¹ u nas zarejestrowane a nie ma ich w raporcie uwa¿amy za niewykonane. Us³ugi, których nie uda³o siê zidentyfikowaæ s¹ tworzone i oznaczane jako wymagaj¹ce wyjaœnienia. Us³ugi zidentyfikowane, gdzie wystêpuj¹ ró¿nice równie¿ oznaczane s¹ jako do wyjaœnienia.
Przegl¹damy takie us³ugi i po wyjaœnieniu z placówk¹ / pacjentem akceptujemy lub odrzucamy.

Klient mo¿e równie¿ do nas zadzwoniæ, ¿eby sprawdziæ czy dany zabieg lub badanie jest pokrywany przez jego ubezpieczenie.

Rozliczenie z placówkami medycznymi odbywa siê w cyklach okreœlonych na umowie (miesiêcznie lub kwartalnie). Za ka¿d¹ zrealizowan¹ us³ugê przekazujemy kwotê wyliczon¹ wczeœniej jako udzia³ ubezpieczyciela. P³atnoœæ odbywa siê na podstawie faktury przekazanej przez placówkê. 

Z ubezpieczycielami rozliczamy siê równie¿ w cyklach okreœlonych na umowie. Wystawimy fakturê zawieraj¹ca koszt wyliczony jako udzia³ ubezpieczyciela we wszystkich sprawach w danym okresie. Faktura rozbita jest na pozycje per produkt ubezpieczyciela i us³uga. Do faktury doliczamy rycza³t za ka¿dego ubezpieczonego zgodnie z stawk¹ na umowie.

Poniewa¿ kontrola kosztów jest dla i naszych klientów niezwykle wa¿na, to chcielibyœmy mieæ mo¿liwoœæ wykrywania nadu¿yæ np. du¿a czêstotliwoœæ korzystania z danej us³ugi lub dziwne po³¹czenie diagnozy z us³ug¹.


Sposób wyliczenia podzia³u kosztu us³ugi:
sprawdzenie, czy na polisie w ochronach jest us³uga o kodzie z pozycji, jeœli nie ma to ca³y koszt ponosi ubezpieczony,
system wylicza udzia³ w³asny na podstawie definicji co-payment z polisy (procentowy lub kwotowy)
nastêpnie pozosta³a kwota porównywana jest z limitem na dan¹ us³ugê i dotychczasowym zu¿yciem tego limitu

Przyk³ad limitu:  "limit" : max koszt 100 PLN, okres rozliczenia rok polisowy oznacza, ¿e w ci¹gu roku polisowego ubezpieczonemu przys³uguje zwrot maksymalnie 100 PLN, ale ma nieograniczon¹ liczbê wyst¹pienia us³ugi. 

W ten sposób powstaje kwota do zap³aty przez ubezpieczyciela.
