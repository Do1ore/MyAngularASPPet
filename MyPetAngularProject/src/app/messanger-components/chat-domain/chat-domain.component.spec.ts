import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatDomainComponent } from './chat-domain.component';

describe('ChatDomainComponent', () => {
  let component: ChatDomainComponent;
  let fixture: ComponentFixture<ChatDomainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatDomainComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatDomainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
