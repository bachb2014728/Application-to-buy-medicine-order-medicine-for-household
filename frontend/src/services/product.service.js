import createApiClient from "./api.service.js";
class ProductClientService {
    constructor(baseUrl = "http://localhost:5086/api/v1/products") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/",{}));
    }
    async create(data){
        return (await this.api.post("/",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async getOne(id){
        return (await this.api.get(`/${id}`,{
            headers: {
                // 'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async updatePriceAndSale(data,id){
        return(await this.api.put(`/edit-price/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async updateQuantity(data,id){
        return(await this.api.put(`/edit-quantity/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async getAllByStore(url){
        return (await this.api.get(`/get-all-product-by-store/${url}`))
    }
    async getOneByURL(url){
        return (await this.api.get(`get-by-url/${url}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async update(id,data){
        return (await this.api.put(`/${id}`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async updateByURL(url,data){
        return (await this.api.put(`URL/${url}/info`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async deleteOne(id){
        return (await this.api.delete(`/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async changeStatus(id,data){
        return (await this.api.put(`${id}/change-status`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new ProductClientService();