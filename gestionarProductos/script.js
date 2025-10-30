// URL de la API
const API_URL = 'https://localhost:7107/api/Productos';

const formProducto = document.getElementById('formProducto');
const tablaProductos = document.getElementById('tablaProductos').getElementsByTagName('tbody')[0];
const inputIdProducto = document.getElementById('productoId');
const inputNombre = document.getElementById('nombre');
const inputPrecio = document.getElementById('precio');
const inputStock = document.getElementById('stock');
const formTitulo = document.getElementById('formTitulo');

window.onload = cargarProductos;

function cargarProductos() {
    fetch(API_URL)
        .then(respuesta => respuesta.json())
        .then(productos => {
            tablaProductos.innerHTML = ''; // Limpiar la tabla
            productos.forEach(producto => {
                const row = tablaProductos.insertRow();
                row.innerHTML = `
                    <td>${producto.nombre}</td>
                    <td>${producto.precio}</td>
                    <td>${producto.stock}</td>
                    <td>
                        <button class="buttonEditar" onclick="editarProducto(${producto.id})">Editar</button>
                        <button class="buttonEliminar" onclick="eliminarProducto(${producto.id})">Eliminar</button>
                    </td>
                `;
            });
        })
        .catch(error => {
            alert("Error al cargar los productos: " + error);
        });
}

// Función para verificar si es crear o actualizar un producto
formProducto.addEventListener('submit', function (event) {
    event.preventDefault();

    const producto = {
        id: inputIdProducto.value ? parseInt(inputIdProducto.value) : undefined,
        nombre: inputNombre.value,
        precio: parseFloat(inputPrecio.value),
        stock: parseInt(inputStock.value)
    };

    if (producto.id) {
        actualizarProducto(producto);
    } else {
        crearProducto(producto);
    }
});

function crearProducto(producto) {
    fetch(API_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(producto)
    })
        .then(respuesta => {
            if (respuesta.status === 204) {
                alert('Producto guardado exitosamente.');
                cargarProductos(); // Recargar la lista de productos
                limpiarForm(); // Limpiar el formulario
                return null;
            }
            if (!respuesta.ok) {
                // Manejar otros errores (4xx, 5xx)
                return respuesta.json().then(data => {
                    // mensaje de la API
                    throw new Error(data.message || `Error ${respuesta.status}`);
                });
            }
            return respuesta;
        })
        .catch(error => {
            alert("Error al crear el producto: " + error.message);
        });
}

function actualizarProducto(producto) {
    fetch(`${API_URL}/${producto.id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(producto)
    })
        .then(respuesta => {
            if (respuesta.status === 204) {
                alert('Producto actualizado exitosamente.');
                cargarProductos();
                limpiarForm();
                return null;
            }
            if (!respuesta.ok) {
                return respuesta.json().then(data => {
                    throw new Error(data.message || `Error ${respuesta.status}`);
                });
            }
            return respuesta;
        })
        .catch(error => {
            alert("Error al actualizar el producto: " + error.message);
        });
}

function eliminarProducto(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este producto?')) {
        fetch(`${API_URL}/${id}`, {
            method: 'DELETE'
        })
            .then(() => {
                if (respuesta.status === 204) {
                    alert('Producto Eliminado exitosamente.');
                    cargarProductos();
                    limpiarForm();
                    return null;
                }
                if (!respuesta.ok) {
                    return respuesta.json().then(data => {
                        throw new Error(data.message || `Error ${respuesta.status}`);
                    });
                }
                return respuesta;
            })
            .catch(error => {
                alert("Error al eliminar el producto:" + error.message);
            });

    }
}

// Función para llenar el formulario con los datos de un producto para editarlo
function editarProducto(id) {
    fetch(`${API_URL}/${id}`)
        .then(respuesta => respuesta.json())
        .then(producto => {
            inputIdProducto.value = producto.id;
            inputNombre.value = producto.nombre;
            inputPrecio.value = producto.precio;
            inputStock.value = producto.stock;
            formTitulo.textContent = 'Editar Producto';
        })
        .catch(error => {
            alert("Error al cargar el producto para editar: " + error);
        });
}

function limpiarForm() {
    inputIdProducto.value = '';
    inputNombre.value = '';
    inputPrecio.value = '';
    inputStock.value = '';
    formTitulo.textContent = 'Agregar Producto';
}
