import { Component, OnInit } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Trade } from '../class/Trade';
import { NewTradeComponent } from '../new-trade/new-trade.component';
import { Portfolio } from '../class/Portfolio';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './edit-portfolio.component.html',
  styles: [``],
  styleUrls: ['./edit-portfolio.component.css']
})
export class EditPortfolioComponent implements OnInit {
  portfolioId: number;
  openPositions: Trade[];
  completedTrades: Trade[];
  portfolio: Portfolio;
  barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  barChartLabels = [];
  barChartType = 'line';
  barChartLegend = false;
  amount: string;
  textAmount: string;

  barChartData = [
    { data: [], label: 'Profit' }
  ];
  constructor(private portfolioService: PortfolioService, private modalService: NgbModal,
    private route: ActivatedRoute) {
    let portfolioId = this.route.snapshot.paramMap.get('id');
    this.portfolioId = parseInt(portfolioId);
  }

  ngOnInit() {
    this.getPortfolioData(null);
  }
  editTrade(tradeId) {
    const modalRef = this.modalService.open(NewTradeComponent, { size: 'lg' });
    modalRef.componentInstance.tradeId = tradeId;
    modalRef.componentInstance.refreshParent = this.getPortfolioData;
  }
  getPortfolioData(portfolioId) {
    var id = this.portfolioId || portfolioId
    this.portfolioService.getPortfolioDetails(id)
      .subscribe(result => {
        this.portfolio = result
        this.openPositions = result.openPosition;
        this.completedTrades = result.completedTrades;

        this.barChartData[0].data = [0];
        this.barChartLabels = [0]
        var graph = 0;
        this.completedTrades.forEach((trade, index) => {
          graph = graph + trade.profit;
          this.barChartData[0].data.push(graph);
          this.barChartLabels.push(index + 1);
        });
      });
  }

  convertAmount() {
    
  }
}
