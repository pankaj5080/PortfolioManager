import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { PortfolioService } from '../service/portfolio-service.service';
import { Networth } from '../class/Networth';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  templateUrl: './networth.component.html',
  styleUrls: ['./networth.component.css']
})
export class NetworthComponent implements OnInit {
  networth: Networth;
  networthForm: FormGroup;

  date: FormControl;
  icici: FormControl;
  upstox: FormControl;
  zerodha: FormControl;
  fivePaisa: FormControl;
  iim: FormControl;
  loan: FormControl;
  samco: FormControl;

  constructor(private portfolioService: PortfolioService, private router: Router) {
    this.date = new FormControl('');
    this.icici = new FormControl('');
    this.upstox = new FormControl('');
    this.zerodha = new FormControl('');
    this.fivePaisa = new FormControl('');
    this.iim = new FormControl('');
    this.loan = new FormControl('');
    this.samco = new FormControl('');

  }

  ngOnInit() {
    this.networthForm = new FormGroup({
      date: this.date,
      icici: this.icici,
      upstox: this.upstox,
      zerodha: this.zerodha,
      fivePaisa: this.fivePaisa,
      iim: this.iim,
      samco: this.samco,
      loan: this.loan
    });

  }
  addNetworth(networthForm) {
    let networth: Networth = {
      date: networthForm.date,
      fivePaisa: networthForm.fivePaisa,
      icici: networthForm.icici,
      iim: networthForm.iim,
      loan: networthForm.loan,
      networthId: 0,
      samco: networthForm.samco,
      upstox: networthForm.upstox,
      zerodha: networthForm.zerodha
    };
    this.portfolioService.addNetworth(networth)
      .subscribe(result => {
        this.router.navigate(['dashboard']);
      });
  }

}
