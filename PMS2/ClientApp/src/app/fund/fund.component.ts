import { Component, OnInit } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service';
import { Portfolio } from '../class/Portfolio';

@Component({
  selector: 'app-fund',
  templateUrl: './fund.component.html',
  styleUrls: ['./fund.component.css']
})
export class FundComponent implements OnInit {
  portfolioList: Portfolio[];
  selectedPortfolio: Portfolio;
  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.portfolioService.getPortfolioList()
    .subscribe(result => { this.portfolioList = result; });
  }

}
