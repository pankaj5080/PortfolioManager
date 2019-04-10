import { Component, OnInit } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service';
import { Dashboard } from '../class/Dashboard';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  dashboard: Dashboard
  barChartLabels = [];
  barChartType = 'line';
  barChartLegend = false;
  barChartData = [
    { data: [], label: 'Profit' }
  ];
  barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  constructor(private portfolioService: PortfolioService) { }

  ngOnInit() {
    this.portfolioService.getDashboardDetails()
      .subscribe(result => {
        this.dashboard = result;

        this.barChartData[0].data = [0];
        this.barChartLabels = [0]
        var graph = 0;

        result.allTrades.forEach((trade, index) => {
          graph = graph + trade.profit;
          this.barChartData[0].data.push(graph);
          this.barChartLabels.push(index + 1);
        });

      });
  }

}
