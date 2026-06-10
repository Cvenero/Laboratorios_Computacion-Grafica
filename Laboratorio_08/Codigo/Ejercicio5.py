import cv2

# Abrir la imagen
imagen = cv2.imread("Imagenes/Gato.jpg")

# Dibujar un circulo sobre la cara del gato
# Parametros: imagen, centro (x,y), radio, color BGR, grosor
cv2.circle(imagen, (150, 120), 80, (0, 255, 0), 3)

# Agregar texto descriptivo
# Parametros: imagen, texto, posicion, fuente, escala, color BGR, grosor
cv2.putText(imagen, "Gato", (100, 230), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

# Guardar la imagen
cv2.imwrite("Imagenes/gato_etiquetado.jpg", imagen)
print("Imagen guardada correctamente")

# Mostrar la imagen
cv2.imshow("Gato etiquetado", imagen)
cv2.waitKey(0)
cv2.destroyAllWindows()