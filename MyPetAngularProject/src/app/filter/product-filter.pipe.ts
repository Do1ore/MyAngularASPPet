import {Pipe, PipeTransform} from '@angular/core';
import {Product} from "../models/product";

@Pipe({
  name: 'productFilter'
})
export class ProductFilterPipe implements PipeTransform {

  transform(products: Product[], searchTerm: string): Product[] {
    if (!searchTerm) {
      return products;
    }
    searchTerm = searchTerm.toLowerCase();
    if (products.length != 0) {
      return products.filter(product =>
        product.productExtendedFullName?.toLowerCase().includes(searchTerm) ||
        product.description?.toLowerCase().includes(searchTerm)
      );
    }
    return products;

  }
}
