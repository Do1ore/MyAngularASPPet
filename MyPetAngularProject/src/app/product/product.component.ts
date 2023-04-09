import {Product} from '../models/product';
import {Component, OnInit} from '@angular/core';
import {ProductService} from "src/app/services/products.service";
import {Router} from "@angular/router"

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  products: Product[] = [];
  searchTerm = '';


  constructor(private productService: ProductService) {
  }

  ngOnInit(): void {
    this.productService.getAllProducts()
      .subscribe({
        next: (products) => {
          for(let product of products)
          {
            product.minPrice = parseFloat(Math.round(<number>product.minPrice).toFixed(0));
          }
          this.products = products;
        },
        error: (response) => {
          console.log(response)
        }
      })
  }

  getStarsArray(productId: number | null): string[] {
    let rating: number | null = 0;
    if (productId !== null) {
      rating = this.products[productId].rating;
    }
    if (rating !== null) {
      const starsCount = parseFloat((<number>rating / 10).toFixed(0)) // количество звезд
      const starsArray = new Array(starsCount).fill('star'); // заполнить массив звездами
      return starsArray;
    }
    const starsCount = 0;
    const starsArray = new Array(starsCount).fill('star'); // заполнить массив звездами
    return starsArray;
  }

  getProductRating(id: number): number {
    if (id !== null) {
      if (this.products[id].rating !== null && this.products !== null) {
        return parseFloat((<number>this.products[id].rating / 10).toFixed(1))
      }
    }
    throw new Error("Null reference");
  }

}
