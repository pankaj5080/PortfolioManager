import { Component, OnInit, Input } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service'
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Trade } from '../class/Trade';

@Component({
  selector: 'app-new-trade',
  templateUrl: './new-trade.component.html',
  styles: [`
    `],
  styleUrls: ['./new-trade.component.css']
})
export class NewTradeComponent implements OnInit {
  @Input() public portfolioId;
  @Input() public tradeId;
  @Input() public refreshParent;
  startDate: any;
  sellDate: any;
  trade: Trade;
  
  constructor(public activeModal: NgbActiveModal, public portfolioService: PortfolioService) {
    this.trade = new Trade();
  }

  ngOnInit() {
    if (this.tradeId) {
      this.portfolioService.getTrade(this.tradeId)
        .subscribe(result => {
          this.trade = result;
          let date = new Date(result.buyDate);
          this.startDate = { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDay() }
        });
    } else {
      this.tradeId = 0;
    }
  }

  addTrade() {
    var date = new Date(this.startDate.year, this.startDate.month - 1, this.startDate.day);
    var sellDate = this.sellDate ? new Date(this.sellDate.year, this.sellDate.month - 1, this.sellDate.day): null;
    this.portfolioService.addTrade({
      PortfolioId: this.portfolioId,
      stockName: this.trade.stockName,
      buyDate: date,
      buyPrice: this.trade.buyPrice,
      quantity: this.trade.quantity,
      tradeId: this.trade.tradeId,
      sellDate: sellDate,
      sellPrice: this.trade.sellPrice ? this.trade.sellPrice : null 
    }).subscribe(result => {
      this.activeModal.close('Close click');
      if (this.refreshParent) {
        this.refreshParent(this.trade.portfolioId);
      }
    });
  }

}
