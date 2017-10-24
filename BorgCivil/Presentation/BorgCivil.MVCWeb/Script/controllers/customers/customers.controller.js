(function () {

    angular
        .module('borgcivil')
        .controller('CustomerRegistrationCtrl', CustomerRegistrationCtrl)
        //.controller('DriverRegistrationCtrl', DriverRegistrationCtrl)

    //Inject required stuff as parameters to factories controller function
    CustomerRegistrationCtrl.$inject = ['$scope'];

    /**
   * @name CustomerRegistrationCtrl
   * @param $scope
   */
    function CustomerRegistrationCtrl($scope) {
        //Assign controller scope pointer to a variable
        var vm = this;

      //Site Detail Tab add supervisor
        vm.supervisor = [];
        vm.addPerson = function(){
          vm.supervisor.push({
          'name':vm.name,
          'email':vm.email,
          'mobile':vm.mobile
        });
      }

    //Job Tab Selct list filter options
  $scope.filterOptions = {
    stores: [
      {id : 2, name : 'Show All', rating: 6 },
      {id : 3, name : 'Rating 5', rating: 5 },
      {id : 4, name : 'Rating 4', rating: 4 },
      {id : 5, name : 'Rating 3', rating: 3 },
      {id : 6, name : 'Rating 2', rating: 2 },
      {id : 7, name : 'Rating 1', rating: 1 } 
    ]
  };
  
  //Contains the sorting options
  $scope.sortOptions = {
    stores: [
      {id : 1, name : 'Price Highest to Lowest' },      
      {id : 2, name : 'Price Lowest to Highest' },
      ]
  };
  
  //Mapped to the model to filter
  $scope.filterItem = {
    store: $scope.filterOptions.stores[0]
  }
  
  //Mapped to the model to sort
  $scope.sortItem = {
    store: $scope.sortOptions.stores[0]
  };
  
  //Watch the sorting model - when it changes, change the
  //ordering of the sort (descending / ascending)
  $scope.$watch('sortItem', function () {
    console.log($scope.sortItem);
    if ($scope.sortItem.store.id === 1) {
      $scope.reverse = true;
    } else {
      $scope.reverse = false;
    }
  }, true);
  
  //Custom filter - filter based on the rating selected
  $scope.customFilter = function (data) {
    if (data.rating === $scope.filterItem.store.rating) {
      return true;
    } else if ($scope.filterItem.store.rating === 6) {
      return true;
    } else {
      return false;
    }
  };  
  
  //The data that is shown
  $scope.data = [
    {
      namee: "product1",
      jobnumber: 198,
      rating: 1,
      testing: 25
    },
    {
      namee: "product2",
      jobnumber: 200,
      rating: 5,
      testing: 55
    },
    {
      namee: "product3",
      jobnumber: 200,
      rating: 2,
      testing: 45
    },
    {
      namee: "product4",
      jobnumber: 10,
      rating: 3,
      testing: 65
    },
    {
      namee: "product5",
      jobnumber: 200,
      rating: 3,
      testing: 245
    },
    {
      namee: "product6",
      jobnumber: 400,
      rating: 5,
      testing: 65
    }
  ];


    };

    

}());
