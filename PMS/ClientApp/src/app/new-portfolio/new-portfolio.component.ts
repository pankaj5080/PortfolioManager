import { Component, OnInit } from '@angular/core';
import { PortfolioService } from '../service/portfolio-service.service'
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PortfolioType } from '../class/PortfolioType';

@Component({
  selector: 'app-new-portfolio',
  templateUrl: './new-portfolio.component.html',
  styles: [`
    `],
  styleUrls: ['./new-portfolio.component.css']
})
export class NewPortfolioComponent implements OnInit {
  portfolioForm: FormGroup;

  name: FormControl;
  startAmount: FormControl;
  position: FormControl;
  selectedPortfolioType: FormControl;

  portfolioTypes: PortfolioType[];

  constructor(private portfolioService: PortfolioService, private router: Router) {
    this.portfolioTypes = [];
  }

  ngOnInit() {
    this.name = new FormControl('', Validators.required);
    this.startAmount = new FormControl('', Validators.required) ;
    this.position = new FormControl('');
    this.selectedPortfolioType = new FormControl({ value: '' });

    this.portfolioForm = new FormGroup({
      portfolioName: this.name,
      startAmount: this.startAmount,
      position: this.position,
      selectedPortfolioType: this.selectedPortfolioType
    });

    this.portfolioService.getPortfolioTypes()
      .subscribe( result => {
        this.portfolioTypes = result;
      });
  }

  isPortfolioNameValid() {
    return this.name.valid || this.name.untouched;
  }

  isStartAmountValid() {
    return this.startAmount.valid || this.startAmount.untouched;
  }

  createPortfolio(portfolioForm) {
    this.portfolioForm.controls.portfolioName.markAsTouched();
    this.portfolioForm.controls.startAmount.markAsTouched();

    if (this.portfolioForm.valid) {
      this.portfolioService.createPortfolio({
        Name: portfolioForm.portfolioName,
        InitialAmount: portfolioForm.startAmount,
        PortfolioTypeId: portfolioForm.selectedPortfolioType,
        Position: portfolioForm.position
      }).subscribe(res => {
        this.router.navigate(['portfolio-list']);
      });
    }
  }

}
