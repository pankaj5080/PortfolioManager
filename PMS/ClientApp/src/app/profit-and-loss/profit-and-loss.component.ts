import { Component, OnInit } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service'
import { ProfitAndLoss } from '../class/ProfitAndLoss';
import { Portfolio } from '../class/Portfolio';

@Component({
  selector: 'app-profit-and-loss',
  templateUrl: './profit-and-loss.component.html',
  styleUrls: ['./profit-and-loss.component.css']
})
export class ProfitAndLossComponent implements OnInit {
  profitNLoss: ProfitAndLoss;
  selectedFinancialYear: string;
  financialYears: string[];
  portfolioList: Portfolio[];
  selectedPortfolio: number;

  constructor(private portfolioService: PortfolioService) {
    this.financialYears = ["2019-20", "2020-21"];
    this.selectedFinancialYear = "2019-20";
  }

  ngOnInit() {
    this.portfolioService.getPortfolioList().subscribe(result => {
      var overallPortfolio : Portfolio[] = [ { portfolioId: 0, name: 'Overall' }];
      this.portfolioList = overallPortfolio.concat(result);
      this.selectedPortfolio = 0;
      this.updatePnl();
    });
  }
  yearChanged(value) {
    this.selectedFinancialYear = value;
    this.updatePnl();
  }
  portfolioChanged(value) {
    this.selectedPortfolio = value;
    this.updatePnl();
  }
  updatePnl() {
    if (this.selectedFinancialYear && (this.selectedPortfolio !== null || this.selectedPortfolio !== undefined)) {
      this.portfolioService.getPnlDetails(this.selectedFinancialYear, this.selectedPortfolio)
        .subscribe(result => {
          this.profitNLoss = result;
      });
    }
  }
  getColor(profit) {
    let color = "black"
    if (profit && profit > 0) {
      color = "green";
    }
    else if (profit && profit < 0) {
      color = "red";
    }
    return color
  }

}
