import { Component, OnInit, Input } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { NewTradeComponent } from '../new-trade/new-trade.component'

@Component({
  selector: 'portfolio-card',
  templateUrl: './portfolio-card.component.html',
  styleUrls: ['./portfolio-card.component.css']
})
export class PortfolioCardComponent implements OnInit {
  constructor(private modalService: NgbModal) { }

  ngOnInit() {
  }

  @Input() portfolio: any;
  addTrade(portfolioId) {
    const modalRef = this.modalService.open(NewTradeComponent, { size: 'lg' });
    modalRef.componentInstance.portfolioId = portfolioId;
  }
}
