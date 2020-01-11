route resolver aloows us to avoid using the safe navigation operator "?" 
eg: object?.property

we typically use elvis(?) to allow our property to not throw exception during the page load in angular.
However, route resolver will help us to avoid using ? at all