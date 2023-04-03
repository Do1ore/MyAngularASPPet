import { Product } from '../models/product';
import { Component, OnInit } from '@angular/core';
import { ProductService } from "src/app/services/products.service";

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  products: Product[] = [];
  searchTerm = '';
  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getAllProducts()
    .subscribe({
      next: (products) =>{
        this.products = products;
      },
      error: (response) =>
      {
        console.log(response)
      }
    })
  }

}
