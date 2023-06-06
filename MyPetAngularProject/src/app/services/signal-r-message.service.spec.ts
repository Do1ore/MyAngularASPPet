import {TestBed} from '@angular/core/testing';

import {SignalRMessageService} from './signal-r-message.service';

describe('SignalRMessageService', () => {
  let service: SignalRMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalRMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
