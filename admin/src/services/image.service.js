import createApiClient from "./api.service.js";
class ImageService {
    constructor(baseUrl = "http://localhost:5086/api/v1/images") {
        this.api = createApiClient(baseUrl);
        this.token = sessionStorage.getItem('token');
    }
    async uploadImage(data) {
        return (await this.api.post("/", data,{
            headers: {
                'Authorization': `Bearer ${this.token}`,
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
    async getOne(id){
        return (await this.api.get(`${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`,
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
    async delete(id){
        return (await this.api.delete(`${id}`,{
            headers: {
                'Authorization': `Bearer ${this.token}`,
                'Content-Type': 'multipart/form-data'
            }
        }));
    }
}
export default new ImageService();