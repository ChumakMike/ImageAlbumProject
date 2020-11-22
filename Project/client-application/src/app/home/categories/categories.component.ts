import { Component, OnInit } from '@angular/core';
import { CategoryModel } from 'src/app/models/category.model';
import { CategoryService } from 'src/app/shared/category.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  categories : CategoryModel[];
  
  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.loadAllCategories();
  }

  loadAllCategories() {
    this.categoryService.getAllCategories().subscribe(
      (res:any) => {
        this.categories = res;
      },
      err => {
        console.log(err);
      }
    )
  }

}
