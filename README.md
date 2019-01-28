# DDD Workshop - TPA for medical policies

<p align="center">
    <img alt="Event Storming Big Picture" src="https://raw.githubusercontent.com/asc-lab/ddd-workshop-tpa/master/readme-images/event_storming_big_picture.png" />
</p>

<p align="center">
    <img alt="Event Storming Design Level" src="https://raw.githubusercontent.com/asc-lab/ddd-workshop-tpa/master/readme-images/event_storming_design_level.png" />
</p>

<p align="center">
    <img alt="Examples" src="https://raw.githubusercontent.com/asc-lab/ddd-workshop-tpa/master/readme-images/examples.png" />
</p>

## TPA OK - opis domeny

Nasza firma TPA OK zajmuje się administracją polisami. Podpisuje umowę z towarzystwem ubezpieczeniowym na obsługę szkód medycznych z polis sprzedawanych przez towarzystwo.

Towarzystwa dostarczają nam dane polis oraz zmian na polisach w postaci plików, które przesyłane są do nas z różną częstotliwością. Niektóre towarzystwa wysyłają plik raz dziennie, inne w cyklach tygodniowych.
Staramy się, negocjując umowy zapewnić, że towarzystwo przekaże nam dane w ustalonym przez nas wspólnym formacie.
W pliku [examples/policy.json](examples/policy.json) przykład fragmentu takiego pliku zawierającego jedną polisę.

Otrzymujemy numer polisy, kod ubezpieczyciela, dane ubezpieczonego, okres obowiązywania polisy, datę zmiany (dla nowej polisy powinna zawsze być równa dacie początku obowiązywania), kod produktu oraz listę ochron. Ochrony określają jakie usługi medyczne są objęte polisą.
Dla każdej usługi może być określona współpłatność (procentowa lub kwotowa) oraz limit. Współpłatność to konkretna kwota lub procent ceny usługi, jaką pacjent musi zapłacić sam przy każdej usłudze. Limit oznacza ilość usług / koszt jaki pokrywa ubezpieczyciel. Limit ma określony czas po którym jest odnawiany.
W chwili obecnej obsługujemy limity na rok polisowy, na szkodę i rok kalendarzowy.  
Limit może dotyczyć pojedynczego ubezpieczonego bądź być wspólny dla wszystkich osób na polisie.

Nasza firma podpisuje umowy z placówkami medycznymi i sieciami takich placówek na świadczenie usług medycznych. Umowa taka zawierana jest na czas określony (zazwyczaj na rok). Umowa zawiera listę placówek oraz cennik usług. Ceny mogą się zmieniać w trakcie obowiązywania umowy.
Niektóre placówki mogą mieć wspólne cenniki, na przykład wszystkie placówki MedHelp w Warszawie mają wspólny cennik. Rozliczenia z placówkami odbywają się według tych cenników. 

Pacjenci zgłaszają się do placówek lub dzwonią na nasze call center. Musimy teraz dokonać autoryzacji wizyty osoby w placówce medycznej. W tym celu musimy wyszukać daną osobę i sprawdzić czy ma ona aktywną polisę. Jeśli nie mamy danych takiej osoby lub nie ma ona aktywnej polisy to informujemy o tym i nie obsługujemy dalej. Następnie w przypadku call center musimy wyszukać placówkę. Dopuszczamy sytuację, że wizyta odbywa się w placówce, z którą nie mamy umowy. Rejestrujemy sprawę i zbieramy informację o planowanych usługach: kod usługi, cena i ilość usług. Mając te informacje możemy znając historię ubezpieczenia obliczyć ile powinien zapłacić pacjent, ile powinien pokryć ubezpieczyciel. Użytkownik z odpowiednimi kompetencjami może dokonać manualnej korekty takiej kalkulacji, w tym odmówić pokrycia kosztu usługi. Musi on w takiej sytuacji podać powód swojej decyzji. W przypadku placówek, z którymi mamy podpisaną umowę cena nie może przekraczać ceny ustalonej w cenniku. Taka wizyta może teraz zostać zaakceptowana lub odrzucona przez pacjenta. 
Po wykonaniu zabiegu placówka potwierdza (chcielibyśmy mieć to w systemie) wykonanie lub pacjent przysyła do nas fakturę za wizytę w przypadku placówek z którymi nie mamy umowy). W przypadku faktury musimy wyszukać sprawę i sprawdzić czy koszt zgodny jest z naszymi wyliczeniami. Jeśli nie, musimy sprawę wyjaśnić z placówką medyczną i dokonać korekty. 
Placówki, z którymi mamy podpisane umowy przesyłają nam raporty wykonanych usług (w cyklach tygodniowych lub miesięcznych). Taki raport musi zostać zweryfikowany ze stanem po naszej stronie. Report zawiera kody usług, daty realizacji, cenę, dane identyfikujące pacjenta i numer polisy. Usługi, które są u nas zarejestrowane a nie ma ich w raporcie uważamy za niewykonane. Usługi, których nie udało się zidentyfikować są tworzone i oznaczane jako wymagające wyjaśnienia. Usługi zidentyfikowane, gdzie występują różnice również oznaczane są jako do wyjaśnienia.
Przeglądamy takie usługi i po wyjaśnieniu z placówką / pacjentem akceptujemy lub odrzucamy.

Klient może również do nas zadzwonić, żeby sprawdzić czy dany zabieg lub badanie jest pokrywany przez jego ubezpieczenie.

Rozliczenie z placówkami medycznymi odbywa się w cyklach określonych na umowie (miesięcznie lub kwartalnie). Za każdą zrealizowaną usługę przekazujemy kwotę wyliczoną wcześniej jako udział ubezpieczyciela. Płatność odbywa się na podstawie faktury przekazanej przez placówkę. 

Z ubezpieczycielami rozliczamy się również w cyklach określonych na umowie. Wystawimy fakturę zawierająca koszt wyliczony jako udział ubezpieczyciela we wszystkich sprawach w danym okresie. Faktura rozbita jest na pozycje per produkt ubezpieczyciela i usługa. Do faktury doliczamy ryczałt za każdego ubezpieczonego zgodnie z stawką na umowie.

Ponieważ kontrola kosztów jest dla i naszych klientów niezwykle ważna, to chcielibyśmy mieć możliwość wykrywania nadużyć np. duża częstotliwość korzystania z danej usługi lub dziwne połączenie diagnozy z usługą.


Sposób wyliczenia podziału kosztu usługi:
sprawdzenie, czy na polisie w ochronach jest usługa o kodzie z pozycji, jeśli nie ma to cały koszt ponosi ubezpieczony,
system wylicza udział własny na podstawie definicji co-payment z polisy (procentowy lub kwotowy)
następnie pozostała kwota porównywana jest z limitem na daną usługę i dotychczasowym zużyciem tego limitu

Przykład limitu:  "limit" : max koszt 100 PLN, okres rozliczenia rok polisowy oznacza, że w ciągu roku polisowego ubezpieczonemu przysługuje zwrot maksymalnie 100 PLN, ale ma nieograniczoną liczbę wystąpienia usługi. 

W ten sposób powstaje kwota do zapłaty przez ubezpieczyciela.
