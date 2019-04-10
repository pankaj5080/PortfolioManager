import { Component, OnInit } from '@angular/core';
import { PortfolioService} from '../service/portfolio-service.service'
import { Portfolio } from '../class/Portfolio';

@Component({
  selector: 'portfolio-list',
  templateUrl: './portfolio-list.component.html',
  styleUrls: ['./portfolio-list.component.css']
})
export class PortfolioListComponent implements OnInit {
  portfolios: Portfolio[];
  constructor(private portfolioService: PortfolioService) {
    this.portfolios = [];
   }

  ngOnInit() {
    this.portfolioService.getPortfolioList()
      .subscribe(result => {
        this.portfolios = result;
      });
  }


}
