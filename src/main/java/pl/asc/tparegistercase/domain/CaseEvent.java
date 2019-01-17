package pl.asc.tparegistercase.domain;

import java.time.LocalDateTime;

//todo child implementations
abstract class CaseEvent {

    protected LocalDateTime eventTime;

    CaseEvent() {
        this.eventTime = LocalDateTime.now();
    }


}
