import createApiClient from "./api.service.js";
class OrderService {
    constructor(baseUrl = "http://localhost:5086/api/v1/orders") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async getAllOfCustomer(){
        return (await this.api.get("/my-list-order",{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async getAllOrderOfStore(id){
        return (await this.api.get(`/get-all-order-of-store/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
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
    async deleteOne(id){
        return (await this.api.delete(`/${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async changeStatusOrder(id,data){
        return (await this.api.put(`/${id}/change-status-order`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async cancelOrder(id,data){
        return (await this.api.put(`/${id}/order-cancel`,data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
}
export default new OrderService();