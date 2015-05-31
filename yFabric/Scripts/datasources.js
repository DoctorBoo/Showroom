

var datasources = (function () {
	var readDatasource = new kendo.data.DataSource({
			transport: {
				read: {
					url: "api/tables",
					type: "get",
					dataType: "json"
				}
			},
			pageSize: 10,
			batch: false,
			//schema: {
			//	model: {
			//		id: "elts",
			//		fields: {
			//			_Id: { editable: false, nullable: false }
			//		}
			//	}
			//},
			schema: {
				parse: function (response) {
					var restaurants = [];
					for (var i = 0; i < response.length; i++) {
						var address = JSON.parse(response[i].elts[1].value);
						var restaurant = {
							id: response[i].elts[0].value,
							name: response[i].elts[5].value,						
							location: address.street + ' ' + address.zipcode + ' ' + address.building
						};
						restaurant[response[i].elts[3].name] = JSON.parse(response[i].elts[3].value);
						restaurant[response[i].elts[4].name] = JSON.parse(response[i].elts[4].value
							.replace(new RegExp('ISODate', 'g'), '') //014-07-18T00:00:00Z
							.replace(new RegExp('\[()]', 'g'), ''));

						restaurants.push(restaurant);
					}
					return restaurants;
				}
			}
		});
	
	$(function () {
		$(".grid2").kendoGrid({
			dataSource: readDatasource,
			pageable: true,
			columns: [
			{
				field: "id",
				title: "Id"
			},
			{
				field: "name",
				title: "Name"
			},
			{
				field: "cuisine",
				title: "Cuisine"
			},
			{
				field: "grades",
				title: "Grades",
				type: "array"
			},
			{
				field: "location",
				title: "Location"
			}],
			success: function (d) {
				console.log(d);
			}
		});
	//	$.ajax({ type: 'get', url: 'api/tables' }).done(function (d) {
	//		console.log(d); data = d;
	//	})
	})
	
	return {
		readDataSource: readDatasource
	}
}())