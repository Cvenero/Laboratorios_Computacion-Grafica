import cv2

# Abrir las tres imagenes
img1 = cv2.imread("Imagenes/Aguila.jpg")
img2 = cv2.imread("Imagenes/Gato.jpg")
img3 = cv2.imread("Imagenes/Lobo.jpg")

# Redimensionar al tamaño de la mas grande
mas_grande = max([img1, img2, img3], key=lambda img: img.shape[0] * img.shape[1])
ancho = mas_grande.shape[1]
alto = mas_grande.shape[0]

img1 = cv2.resize(img1, (ancho, alto))
img2 = cv2.resize(img2, (ancho, alto))
img3 = cv2.resize(img3, (ancho, alto))

# Extraer canales (OpenCV usa BGR)
canal_rojo  = img1[:, :, 2]  # Rojo de Aguila
canal_verde = img2[:, :, 1]  # Verde de Gato
canal_azul  = img3[:, :, 0]  # Azul de Lobo

# Combinar en una nueva imagen
imagen_combinada = cv2.merge([canal_azul, canal_verde, canal_rojo])

# Guardar
cv2.imwrite("Imagenes/combinada.jpg", imagen_combinada)
print("Imagen combinada guardada correctamente")

# Mostrar
cv2.imshow("Imagen Combinada", imagen_combinada)
cv2.waitKey(0)
cv2.destroyAllWindows()